﻿using HoloToolkit;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity.InputModule;


namespace HoloToolkit.Unity.InputModule
{



    public class Dictationizer : MonoBehaviour
    {
        private DictationRecognizer dictationRecognizer;


        public Text DictationDisplay;
        private StringBuilder textSoFar;
        public annotationManager annotMananger;
        public KeywordManager keyWordManager;
        bool inProgress;


        // Use this for initialization
        void Start()
        {
            inProgress = false;
            keyWordManager = GameObject.Find("InputManager").GetComponent<KeywordManager>();
            annotMananger = GameObject.Find("AnnotationManager").GetComponent<annotationManager>();








        }





        // Update is called once per frame
        void Update()
        {

        }

        public void setUpDictation()
        {

            dictationRecognizer = new DictationRecognizer();
            if (!inProgress)
            {
                DictationDisplay.text = "Initializing...";
                textSoFar = new StringBuilder();
                keyWordManager.keywordRecognizer.Stop();
                keyWordManager.keywordRecognizer.Dispose();
                PhraseRecognitionSystem.Shutdown();
                dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;
                dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
                dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
                dictationRecognizer.DictationError += DictationRecognizer_DictationError;
                dictationRecognizer.Start();
                DictationDisplay.text = "Speech to text started.  Begin dictating";
                inProgress = true;
            }
            
        }

        private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
        {
            textSoFar.Append(text + ". ");
            DictationDisplay.text = textSoFar.ToString();
        }

        private void DictationRecognizer_DictationHypothesis(string text)
        {
            DictationDisplay.text = textSoFar.ToString() + " " + text + "...";
        }

        public void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
        {
            dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;
            dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
            dictationRecognizer.DictationHypothesis -= DictationRecognizer_DictationHypothesis;
            dictationRecognizer.DictationError -= DictationRecognizer_DictationError;
            dictationRecognizer.Dispose();
        }

        private void DictationRecognizer_DictationError(string error, int hresult)
        {
            DictationDisplay.text = "ERROORRRRR";
        }



        public void stopDiction()
        {
            if (inProgress)
            {
                dictationRecognizer.Stop();
                dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;
                dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
                dictationRecognizer.DictationHypothesis -= DictationRecognizer_DictationHypothesis;
                dictationRecognizer.DictationError -= DictationRecognizer_DictationError;
                dictationRecognizer.Dispose();
                keyWordManager.Start();
                PhraseRecognitionSystem.Restart();
                DictationDisplay.text = "done";
                inProgress = false;
            }



        }




    }
}
