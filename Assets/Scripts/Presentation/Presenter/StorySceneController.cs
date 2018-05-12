﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySceneController : MonoBehaviour
{
    [HideInInspector]
    public StorySceneViewController viewController;
    ScenarioManager scenarioManager;
    TextHelper textHelper;

    bool isDataReady = false;
    bool isClickable = true;

    void Start()
    {
        // 初期データのインスタンスがない場合は、データを再ロードする。
        isDataReady = TextFileHelper.IsExist(Const.Path.MasterData.escapeInput) && ScenarioRepository.Count != 0;
        if (!isDataReady && TextFileHelper.IsExist(Const.Path.MasterData.escapeInput))
        {
            GameController.Instance.Init();
            isDataReady = true;
        }
        viewController = GameObject.FindObjectOfType<StorySceneViewController>();
        scenarioManager = GameObject.FindObjectOfType<ScenarioManager>();
        viewController.Init();
        scenarioManager.Init();
        textHelper = new TextHelper(viewController.contentText);

        if (isDataReady) scenarioManager.Next();
    }

    void Update()
    {
        if (isDataReady) textHelper.Update();
    }

    public void OnClick()
    {
        if (isClickable)
        {
            if (textHelper.IsCompleteDisplayText)
            {
                scenarioManager.Next();
            }
            else
            {
                textHelper.CompleteDisplayText();
            }
        }
    }

    public void ShowNextText(string name, string content)
    {
        viewController.nameText.text = name;
        textHelper.SetNextLine(content);
    }

    public void ShowNextText(string name, string content, float speed)
    {
        viewController.nameText.text = name;
        textHelper.SetNextLine(content, speed);
    }

    public void ShowSelections(List<Scenario> selectionList)
    {
        for (int i = 0; i < selectionList.Count; i++)
        {
            viewController.ToggleSelectionButtonIsActive(i, true);
            viewController.UpdateSelectionText(i, selectionList[i].Text);
        }
        isClickable = false;
    }

    public void OnSelectionSelected(int index)
    {
        // Selectionを非表示にする
        for (int i = 0; i < viewController.NumOfSelectionButtons; i++)
        {
            viewController.ToggleSelectionButtonIsActive(i, false);
        }
        // Jumpさせる
        scenarioManager.OnSelectionSelected(index);
        isClickable = true;
    }
}