using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class InfoCollector : MonoBehaviour {

    static private InfoCollector _instance;
    static public InfoCollector GetInstance()
    {
        return _instance;
    }
    void Awake()
    {
        _instance = this;
    }

    GameManager gameManager;
    PlayerBehaviour player;
    public int playerID;
    string writePath_movement;
    string readPath_movement;
    string writePath_Note;
    string readPath_Note;
    string line;
    int count;
    public List<Vector3> points { get; set; }
    public List<Note> notes { get; set; }
    public bool trackPosition;
    public float trackingDelay;
    private float lastTrackTime;
    public bool drawMovement;
    public bool simulateMovement;
    public bool takePlaytestNotes;
    public bool readPlaytestNotes;
    public Vector3 noteCanvasScale;
    public Vector2 noteCanvasSize;

    public struct Note
    {
        public float Time;
        public Vector3 position;
        public string note;
    }

	// Use this for initialization
	void Start () {
        gameManager = GameManager.GetInstance();
        player = gameManager.player;
        points = new List<Vector3>();
        notes = new List<Note>();

        writePath_movement = "Assets\\DataCollection\\" + playerID + "-movement.txt";
        readPath_movement = "Assets\\DataCollection\\" + playerID + "-movement.txt";
        writePath_Note = "Assets\\DataCollection\\" + playerID + "-notes.txt";
        readPath_Note = "Assets\\DataCollection\\" + playerID + "-notes.txt";

        count = 0;

        if(drawMovement || simulateMovement)
            ReadPositionFile();
        if (readPlaytestNotes)
            ReadNotesFile();
	}

    void Update()
    {
        if (trackPosition && Time.time > lastTrackTime + trackingDelay)
        {
            WritePosition();
            lastTrackTime = Time.time;
        }
        
        if(drawMovement)
            DrawMovement();

        if (simulateMovement)
            RedoMovement();

    }

    void WritePosition()
    {
        StreamWriter writeFile = new StreamWriter(writePath_movement, true);
        if (writeFile != null)
            Debug.Log("File Ok");

        line = player.transform.position.ToString();
        line = line.Replace("(", "");
        line = line.Replace(")", "");

        Debug.Log(line);

        writeFile.WriteLine(line);
        writeFile.Close();
    }

    void ReadPositionFile()
    {
        StreamReader readFile = new StreamReader(readPath_movement);
        while ((line = readFile.ReadLine()) != null)
        {
            points.Add(StringToVector(line));
        }
        readFile.Close();
    }

    void RedoMovement()
    {
        if(count < points.Count)
            player.transform.position = points[count];

        count++;
    }

    void DrawMovement()
    {
        for (int i = 0; i < points.Count; i++)
        {
            if(i+1 < points.Count)
                Debug.DrawLine(points[i], points[i + 1], Color.red);
        }
    }

    public void WritePlaytestNote(Vector3 position, string note)
    {
        if (readPlaytestNotes)
        {
            StreamWriter writeFile = new StreamWriter(writePath_Note, true);

            line = player.transform.position.ToString();
            line = line.Replace("(", "");
            line = line.Replace(")", "");
            line += "#" + Time.time.ToString() + "#" + note;
            writeFile.WriteLine(line);
            writeFile.Close();
        }
    }

    void ReadNotesFile()
    {
        StreamReader readFile = new StreamReader(readPath_Note);
        while ((line = readFile.ReadLine()) != null)
        {
            List<string> noteParts = line.Split('#').ToList();
            Note note = new Note();
            note.position = StringToVector(noteParts[0]);
            note.Time = float.Parse(noteParts[1]);
            note.note = noteParts[2];

            notes.Add(note);
        }
        readFile.Close();
    }

    Vector3 StringToVector(string str)
    {
        List<string> vect = str.Split(',').ToList();
        Vector3 result = new Vector3(float.Parse(vect[0]), float.Parse(vect[1]), float.Parse(vect[2]));
        return result;
    }


    //BUG :D
    void DrawNotes()
    {
        Debug.Log("draw");
        foreach (Note n in notes)
        {
            Canvas noteCanvas = new Canvas();
            //noteCanvas.renderMode = RenderMode.WorldSpace; // <--- Bug : not found
            noteCanvas.transform.position = n.position;
            noteCanvas.GetComponent<RectTransform>().sizeDelta = noteCanvasSize;
            noteCanvas.gameObject.AddComponent<Text>();
            noteCanvas.GetComponent<Text>().text = n.note;

            Debug.Log("Note Drawn");
        }
    }
}
