using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace WorkBrew
{
    public class UIController : MonoBehaviour
    {

        [Header("PrefabsOnGame")]
        public GameObject ClassesButton;

        [Header("UI Elements")]
        public Text TimeUI;
        public Text DateUI;
        public Text HeadlineWeeklySchedule;

        public GameObject Main;
        public GameObject Timetable;
        public GameObject LessonSchedule;
        public GameObject ExtraClasses;
        public GameObject WeeklySchedule;
        [Space]
        public GameObject ExtraClassesX4;
        public GameObject ExtraClassesX3;

        public GameObject DatePanel;

        bool BlindMode = false;

        #region MonoBehaviour Methods
        void Awake()
        {
            RefreshUI(false);
            HideAll();
            Main.SetActive(true);
            DatePanel.SetActive(true);
            GenerateClassesButtons();
        }

        void FixedUpdate()
        {
            string day = DateTime.Now.DayOfWeek.ConvertToString();
            TimeUI.text = DateTime.Now.ToString("HH:mm ") + day;
            DateUI.text = DateTime.Now.ToString("dd.MM.yyyy");
        }
        #endregion

        #region Public Void
        public void ClickClass(GameObject ClassButton)
        {
            if (ClassButton.name == "Back")
            {
                OnLessonSchedule(true);
                return;
            }
            HideAll();
            WeeklySchedule.SetActive(true);
            DatePanel.SetActive(true);
            HeadlineWeeklySchedule.text = DateTime.Now.DayOfWeek.ConvertToString(false) + " " + ClassButton.name;
        }

        public void ClickClassDay(int id)
        {
            string NameClass = HeadlineWeeklySchedule.text.Split()[1];
            HeadlineWeeklySchedule.text = ((DayOfWeek)id).ConvertToString(false) + " " + NameClass;
        }

        public void OnTimetable(bool Open)
        {
            HideAll();
            Timetable.SetActive(Open);
            DatePanel.SetActive(true);
            Main.SetActive(!Open);
        }

        public void OnLessonSchedule(bool Open)
        {
            HideAll();
            LessonSchedule.SetActive(Open);
            DatePanel.SetActive(!Open);
            Main.SetActive(!Open);
        }

        public void OnExtraClasses(bool Open)
        {
            HideAll();
            ExtraClasses.SetActive(Open);
            Main.SetActive(!Open);
        }

        public void OnClickBlindMode()
        {
            RefreshUI(!BlindMode);
        }
        #endregion

        #region Private Void
        void HideAll()
        {
            Main.SetActive(false);
            Timetable.SetActive(false);
            LessonSchedule.SetActive(false);
            ExtraClasses.SetActive(false);
            DatePanel.SetActive(false);
            WeeklySchedule.SetActive(false);
        }

        void GenerateClassesButtons()
        {
            int ASCIICodeA = 'А';
            for (int i = 1; i < 20; i++)
            {
                string name = (i / 4 + 5).ToString() + Convert.ToChar(i % 4 + ASCIICodeA);

                GameObject Button = Instantiate(ClassesButton, ExtraClassesX4.transform);
                Button.GetComponentInChildren<Text>().text = name;
                Button.name = name;
            }
            for (int i = 0; i < 6; i++)
            {
                string name = (i / 3 + 10).ToString() + Convert.ToChar(i % 3 + ASCIICodeA);

                GameObject Button = Instantiate(ClassesButton, ExtraClassesX3.transform);
                Button.GetComponentInChildren<Text>().text = name;
                Button.name = name;
            }
        }

        void RefreshUI(bool StateBlindMode)
        {
            BlindMode = StateBlindMode;
        }
        #endregion

    }
}