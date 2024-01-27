using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mediapipe;
using Mediapipe.Unity;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;


public class HandDetector : MonoBehaviour
{
    private const int HandSegments = 21;

    [SerializeField] private TextAsset configText;
    [SerializeField] private int width = 1280;
    [SerializeField] private int height = 720;
    [SerializeField] private int fps = 30;
    [SerializeField] private GameObject fingerPrefab;
    [SerializeField] private GameObject handWarning;
    [SerializeField] private float smoothingFactor = 0.5f;
    [SerializeField] private float handMinDistance = 0.5f;

    private OutputStream<List<NormalizedLandmarkList>> _landmarksStream;
    private OutputStream<ImageFrame> _outputVideoStream;
    private ResourceManager _resourceManager;
    private LineRenderer _lineRenderer;
    private CalculatorGraph _graph;
    private Transform[] _fingers;
    private Stopwatch _stopwatch;

    private WebCamTexture _webCamTexture;
    private Texture2D _inputTexture;
    private Color32[] _inputPixelData;

    private static Vector3 LandmarkToWorldPoint(NormalizedLandmark landmark)
    {
        var main = Camera.main!;
        return main.ViewportToWorldPoint(new Vector3(landmark.X, landmark.Y, 0)) - main!.transform.position;
    }

    private void ProcessHand(NormalizedLandmarkList hand)
    {
        var center = LandmarkToWorldPoint(hand.Landmark[0]);
        var reference = LandmarkToWorldPoint(hand.Landmark[1]);
        var dist = 1 / Vector3.Distance(center, reference);
        var goodHand = dist >= handMinDistance;

        ToggleHandWarning(!goodHand);
        if (!goodHand) return;

        var pairs = hand.Landmark.Select((v, i) => (i, v /*, _fingers[i]*/)).ToArray();

        foreach (var (i, landmark /*, finger*/) in pairs)
        {
            var point = LandmarkToWorldPoint(landmark);

            // finger.up = center - point;
            // finger.position = Vector3.Lerp(finger.position, point /* *dist */, Time.deltaTime * 1000 * smoothingFactor);
            // finger.position = point * dist;

            var position = _lineRenderer.GetPosition(i);
            var smoothed = Vector3.Lerp(position, point, Time.deltaTime * 1000 * smoothingFactor);
            _lineRenderer.SetPosition(i, smoothed);
        }
    }

    private void ToggleHandVisibility(bool visible)
    {
        // if (_fingers[0].gameObject.activeSelf == visible) return;

        _lineRenderer.enabled = visible;
        // foreach (var finger in _fingers)
        //     finger.gameObject.SetActive(visible);
    }

    private void ToggleHandWarning(bool visible)
    {
        Time.timeScale = visible ? 0 : 1;
        handWarning.SetActive(visible);
    }

    private void ProcessHands(List<NormalizedLandmarkList> hands)
    {
        var doYouHaveHand = hands is { Count: 1 };

        ToggleHandVisibility(doYouHaveHand);
        ToggleHandWarning(!doYouHaveHand);

        if (!doYouHaveHand) return;

        foreach (var hand in hands)
            ProcessHand(hand);
    }

    private IEnumerator DetectHand()
    {
        _inputTexture.SetPixels32(_webCamTexture.GetPixels32(_inputPixelData));
        var currentTimestamp = _stopwatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
        var imageFrame = new ImageFrame(ImageFormat.Types.Format.Srgba, width, height, width * 4,
            _inputTexture.GetRawTextureData<byte>());
        _graph.AddPacketToInputStream("input_video", Packet.CreateImageFrameAt(imageFrame, currentTimestamp));

        var task = _landmarksStream.WaitNextAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (!task.Result.ok)
            throw new Exception("Something went wrong");

        var handLandmarksPacket = task.Result.packet;
        var handLandmarks = handLandmarksPacket?.Get(NormalizedLandmarkList.Parser);

        ProcessHands(handLandmarks);
    }

    private IEnumerator Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        yield return InitWebCam();

        _resourceManager = new StreamingAssetsResourceManager();
        yield return _resourceManager.PrepareAssetAsync("hand_landmark_full.bytes");
        yield return _resourceManager.PrepareAssetAsync("palm_detection_full.bytes");
        yield return _resourceManager.PrepareAssetAsync("handedness.txt");

        _inputTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        _inputPixelData = new Color32[width * height];

        _stopwatch = new Stopwatch();
        _stopwatch.Start();

        InitGraph();

        // _fingers = Enumerable.Range(0, HandSegments)
        //     .Select(_ => Instantiate(fingerPrefab, transform).transform)
        //     .ToArray();

        while (_graph != null)
            yield return DetectHand();
    }

    private IEnumerator InitWebCam()
    {
        if (WebCamTexture.devices.Length == 0) throw new Exception("Web Camera devices are not found");
        var webCamDevice = WebCamTexture.devices[0];
        _webCamTexture = new WebCamTexture(webCamDevice.name, width, height, fps);
        _webCamTexture.Play();

        yield return new WaitUntil(() => _webCamTexture.width > 16);
    }

    private void InitGraph()
    {
        var sidePacket = new PacketMap();
        sidePacket.Emplace("num_hands", Packet.CreateInt(2));
        sidePacket.Emplace("input_horizontally_flipped", Packet.CreateBool(true));
        sidePacket.Emplace("input_vertically_flipped", Packet.CreateBool(false));
        sidePacket.Emplace("input_rotation", Packet.CreateInt(0));

        _graph = new CalculatorGraph(configText.text);
        _landmarksStream = new OutputStream<List<NormalizedLandmarkList>>(_graph, "hand_landmarks");
        _landmarksStream.StartPolling();
        _graph.StartRun(sidePacket);
    }

    private void OnDestroy()
    {
        if (_webCamTexture != null)
            _webCamTexture.Stop();

        if (_graph != null)
        {
            try
            {
                _graph.CloseInputStream("input_video");
                _graph.WaitUntilDone();
            }
            finally
            {
                _graph.Dispose();
                _graph = null;
            }
        }
    }
}