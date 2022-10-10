// by Zaniyar Jahany - 2022
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.SpatialAwareness;
using Microsoft.MixedReality.Toolkit;
using System.Threading;

public class ReadCSV : MonoBehaviour
{
    // Start is called before the first frame update
    private string[] lines;
    private string allText;

    private float timeRemaining = 0;
    public bool timerIsRunning = false;
    // public Text timeText;
    public TextMeshPro timeText;
    public TextMeshPro wordText;

    public AudioSource audioSource;
    public AudioSource audioSourceStart;
    public AudioSource audioSourceStop;
    /* show Keyboard  im start und update code enthalten
    UnityEngine.TouchScreenKeyboard keyboard;
    public static string keyboardText = "";
    */
    async private void playDemoSound(CancellationTokenSource tokenSource)
    {
        await Task.Delay(2000, tokenSource.Token); // warten lassen
        audioSource.Play();
    }
    async void Start()
    {
        Debug.Log("Hello I started");
        var tokenSource = new CancellationTokenSource();

        playDemoSound(tokenSource);
        // tokenSource.Cancel();

        // await Task.Delay(1000); // warten lassen
        // audioSourceStart.Play();

        // await Task.Delay((int)(23030));
        // Debug.Log("XX");
        // keyboard = TouchScreenKeyboard.Open("text to edit"); // Show Keyboard

        // StartCoroutine(Impulser());
        // StartCoroutine(StartCountdown());
        /*
        wordText.SetText("<br>hi<br>");
        var currentShownText = wordText.text;
        Debug.Log(currentShownText);
        currentShownText = currentShownText.Replace("<br>hi", "");
        wordText.SetText(currentShownText);
        currentShownText = wordText.text;
        Debug.Log(currentShownText);
        */
    }

    // // Update is called once per frame
    // IEnumerator Impulser()
    // {
    //     // yield return null;
    //     Debug.Log("Vorher!!!!");
    //     yield return new WaitForSeconds(3f);
    //     Debug.Log("3S Nachher!!!!");
    // }
    // float currCountdownValue;
    // public IEnumerator StartCountdown(float countdownValue = 100)
    // {
    //     currCountdownValue = countdownValue;
    //     while (currCountdownValue > 0)
    //     {
    //         Debug.Log("Countdown: " + currCountdownValue);
    //         yield return new WaitForSeconds(1.0f);
    //         currCountdownValue--;
    //     }
    // }
    // Update is called once per frame
    void Update()
    {
        /* Keyboard 
        keyboardText = keyboard.text;
        wordText.SetText(keyboardText);
        */
        if (timerIsRunning)
        {
            // Debug.Log("OKOK");
            if (timeRemaining < 1000)
            {
                timeRemaining += Time.deltaTime;
                // Debug.Log(timeRemaining);
                DisplayTime(timeRemaining); // Countdown im Textbox anziegen
            }
            else
            {
                Debug.Log("Time has run 100 out!");
                timeRemaining = 100;
                timerIsRunning = false;
            }
        }
    }

    private SpatialAwarenessMeshDisplayOptions[] spatialVisiblityOptions = { SpatialAwarenessMeshDisplayOptions.None, SpatialAwarenessMeshDisplayOptions.Visible };
    private int optionIncrementor = 0;
    public void hideSpatialMesh()
    {
        // Get the first Mesh Observer available, generally we have only one registered
        var observer = CoreServices.GetSpatialAwarenessSystemDataProvider<IMixedRealitySpatialAwarenessMeshObserver>();
        // Set to not visible
        observer.DisplayOption = spatialVisiblityOptions[optionIncrementor % spatialVisiblityOptions.Length];
        optionIncrementor++;
    }


    async public void startTimer(string file)
    {
        // await Task.Delay((int)(23030));
        // Debug.Log("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
        await Task.Delay(1000); // warten lassen
        audioSource.Play();
        await Task.Delay(1000); // warten lassen
        audioSource.Play();
        await Task.Delay(1000); // warten lassen
        audioSourceStart.Play();
        timeRemaining = 0;
        Read(file);
        timerIsRunning = true;
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        // Debug.Log(string.Format("{0:00}:{1:00}", minutes, seconds));
        // timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        timeText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
        // timeText.SetText(timeToDisplay.ToString());
    }
    async void showSubtitleWord(int ms, string word, CancellationTokenSource tokenSource)
    {
        await Task.Delay(ms, tokenSource.Token); // warten lassen
        Debug.Log(word);
        // Debug.Log(word);
        // Debug.Log(m + s);
        var currentShownText = wordText.text;
        if (currentShownText == "" || currentShownText == "<br>")
        {
            wordText.SetText(word);
        }
        else
        {
            currentShownText += "<br>" + word;
            wordText.SetText(currentShownText);
        }
    }
    async void removeSubtitleWord(int ms, string word, CancellationTokenSource tokenSource)
    {
        await Task.Delay(ms, tokenSource.Token); // warten lassen
        var currentShownText = wordText.text;
        currentShownText = currentShownText.Replace("<br>" + word, "");
        currentShownText = currentShownText.Replace(word + "<br>", "");
        currentShownText = currentShownText.Replace(word, "");
        wordText.SetText(currentShownText);
    }
    private CancellationTokenSource tokenSource;
    public void StopSubtitles()
    {
        try
        {
            audioSourceStop.Play();
            tokenSource.Cancel();
            wordText.SetText("");
            timeText.SetText("00:00");
            timerIsRunning = false;
            timeRemaining = 0;
        }
        catch (System.Exception)
        {

            Debug.Log("Something wrong bre!");
        }
    }
    async void Read(string file)
    {
        tokenSource = new CancellationTokenSource();
        wordText.SetText("");
        var fileData = Resources.Load<TextAsset>(file);
        // Debug.Log(fileData.ToString());
        allText = fileData.ToString();
        lines = allText.Split("\n"[0]);
        string[] lineData = (lines[0].Trim()).Split(";"[0]);

        var useStartTS = true; // in the CSV there is on Col H the starting time already in ms
        // float x;
        // float.TryParse(lineData[0], out x);
        foreach (var item in lines)
        {
            if (item.Length > 0)
            {
                var word = item.Trim().Split(";"[0])[1];
                if (useStartTS) //use the already calculated ms times in the csv
                {
                    if (item.Trim().Split(";"[0])[7] != "startTS")
                    {
                        var start_ms = int.Parse(item.Trim().Split(";"[0])[7]);
                        var end_ms = int.Parse(item.Trim().Split(";"[0])[8]);
                        showSubtitleWord(start_ms, word, tokenSource);
                        removeSubtitleWord(end_ms, word, tokenSource);
                    }
                }
                else // use start and end time in format 00:00:00 given in csv
                {
                    var start_time = item.Trim().Split(";"[0])[2];
                    var end_time = item.Trim().Split(";"[0])[3];
                    if (start_time != "start") // überspringe erste zeile
                    {
                        string[] start_timeArray = start_time.Split(':');
                        var start_min = start_timeArray[0];
                        var start_sek = start_timeArray[1];
                        // Debug.Log(min); // start time string
                        // Debug.Log(sek); // start time string

                        var start_m = (int)(float.Parse(start_min) * 60 * 1000);
                        var start_s = (int)(float.Parse(start_sek) * 1000);
                        var start_ms = (int)(start_s + start_m);
                        showSubtitleWord(start_ms, word, tokenSource);
                        //hide the word again: 
                        string[] end_timeArray = end_time.Split(':');
                        var end_min = end_timeArray[1];
                        var end_sek = end_timeArray[2];
                        var end_m = (int)(float.Parse(end_min) * 60 * 1000);
                        var end_s = (int)(float.Parse(end_sek) * 1000);
                        var end_ms = (int)(end_s + end_m);
                        // showSubtitleWord(end_ms, "^^");
                        removeSubtitleWord(end_ms, word, tokenSource);
                    }
                }
            }
        }
    }
}
