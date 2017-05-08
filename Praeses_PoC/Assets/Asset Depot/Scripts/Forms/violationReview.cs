﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity
{
    public class violationReview : MonoBehaviour {

        public Text[] violationData;
        public violationController violationControl;
        public GameObject[] resolutions;
        public GameObject ReviewHolder;
        public GameObject submittedViolationHolder;
        public GameObject SubmittedMainHolder;
        GameObject copiedViolationContent;

    // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void loadReview()
        {
            for(int i=0; i < violationControl.violationData.Count+1; i++)
            {
                if(i!= violationControl.violationData.Count)
                {
                    violationData[i].text = violationControl.violationData[i];
                }
                else
                {
                    int sCounter = 0;
                    int pCounter = 0;
                    int vCounter = 0;
                    string tempString = "";
                    foreach(GameObject activeComment in violationControl.gameObject.GetComponent<commentManager>().activeComments)
                    {
                        if (activeComment.GetComponent<commentContents>().isSimple)
                        {
                            sCounter += 1;
                        }
                        if (activeComment.GetComponent<commentContents>().isVideo)
                        {
                            vCounter += 1;
                        }
                        if (activeComment.GetComponent<commentContents>().isPhoto)
                        {
                            pCounter += 1;
                        }

                    }
                    if (pCounter > 0)
                    {
                        tempString = tempString + "Photo Notes" + "(" + pCounter + ")";
                        if (sCounter > 0)
                        {
                            tempString = tempString + ", ";
                        }
                        if (vCounter > 0)
                        {
                            tempString = tempString + ", ";
                        }
                    }
                    if (sCounter > 0)
                    {
                        tempString = tempString + "Simple Notes" + "(" + sCounter + ")";
                        if (vCounter > 0)
                        {
                            tempString = tempString + ", ";
                        }
                    }
                    if (vCounter > 0)
                    {
                        tempString = tempString + "Video Notes" + "(" + vCounter + ")";
                    }
                    violationData[i].text = tempString;
                }
            }
        }

        public void submitReview(bool fromJson)
        {

            copiedViolationContent = Instantiate(this.gameObject, transform.position, Quaternion.identity);
            copiedViolationContent.transform.SetParent(submittedViolationHolder.transform);
            copiedViolationContent.transform.localScale = transform.localScale;
            copiedViolationContent.transform.localRotation = transform.localRotation;
            violationControl.showTabs(false);

            violationControl.violationHeader.text = violationControl.violationData[2];
            SubmittedMainHolder.SetActive(true);
            ReviewHolder.SetActive(false);

            violatoinSpawner.Instance.populatePreviewField();
            if (!fromJson)
            {
                databaseMan.Instance.syncViolation(violationControl);

            }
        }

        public void enableEditing()
        {
            violationControl.violationHeader.text = "Edit Violation";
            DestroyImmediate(copiedViolationContent);
            violationControl.showTabs(true);
            SubmittedMainHolder.SetActive(false);
            ReviewHolder.SetActive(true);
        }

        public void resolveViolation()
        {
            violatoinSpawner.Instance.violationPreview.GetComponent<viewViolationController>().reorderFields(violationControl.linkedPreview);
            violatoinSpawner.Instance.violationPreview.GetComponent<viewViolationController>().vioResolvedFields.Add(violationControl.linkedPreview);
            violationControl.linkedPreview.transform.localPosition = violatoinSpawner.Instance.violationPreview.GetComponent<viewViolationController>().resolvedPos.localPosition;

            resolutions[0].SetActive(false);
            resolutions[1].SetActive(false);

        }

        public void vioNotResolved()
        {
            resolutions[2].SetActive(false);
            resolutions[1].SetActive(false);
        }
        public void vioNotResolvedOther()
        {
            resolutions[2].SetActive(false);
            resolutions[0].SetActive(false);
        }
        


    }
}