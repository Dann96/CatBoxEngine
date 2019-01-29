using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Localization
{
    /// <summary>
    /// Manages the localization of languages
    /// </summary>
    public class LocalizationManager : PersistentSingleton<LocalizationManager>
    {
        public Font defaultFont;
        private const string LOCALIZATION_REGISTRY_FILE_NAME = "Lang";
        private const string DEFAULT_LOCALIZATION_FILE_NAME = "SOTT_en";
        private const string missingTextString = "Localized Text Not Found";
        private static Dictionary<string, string> currentLocalizedText;
        private static Font currentFont;
        private static List<Dictionary<string, string>> localizedTextList = new List<Dictionary<string, string>>();
        private static List<Font> fontList = new List<Font>();
        private static bool b_LocalizationReady = false;
        private static LocalizationStatus m_Status = LocalizationStatus.validating;

        private static LocalizationFileRegistry m_FileRegistry;

        public delegate void OnLocalizationFileChanged();
        public OnLocalizationFileChanged onLocalizationFileChanged;


        public static bool localizationReady
        {
            get { return b_LocalizationReady; }
        }

        public static LocalizationStatus localizationStatus
        {
            get { return m_Status; }
        }

        public static int localizedTextListCount
        {
            get { return localizedTextList.Count; }
        }

        private void Start()
        {
            PrimeLocalization();
        }

        public static void PrimeLocalization()
        {
            CheckForValidFiles();
        }

        private static void CheckForValidFiles()
        {
            string registryPath = Path.Combine(Application.streamingAssetsPath, LOCALIZATION_REGISTRY_FILE_NAME + ".json");
            if (File.Exists(registryPath))
            {
                string dataAsJson = File.ReadAllText(registryPath);
                try
                {
                    m_FileRegistry = JsonUtility.FromJson<LocalizationFileRegistry>(dataAsJson);
                    Debug.Log("Registry File exists and is readable");

                    LocalizationFileRegistry tempRegist = new LocalizationFileRegistry(new List<LocalizationFileEntry>());
                    for (int i = 0; i < m_FileRegistry.fileEntries.Count; i++)
                    {
                        string filePath = Path.Combine(Application.streamingAssetsPath, m_FileRegistry.fileEntries[i].fileName + ".json");
                        if (File.Exists(filePath))
                        {
                            try
                            {
                                string langDataAsJson = File.ReadAllText(filePath);
                                LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(langDataAsJson);

                                localizedTextList.Add(new Dictionary<string, string>());

                                if (loadedData.font != null)
                                {
                                    fontList.Add(loadedData.font);
                                }
                                else fontList.Add(instance.defaultFont);


                                for (int j = 0; j < loadedData.items.Length; j++)
                                {
                                    localizedTextList[localizedTextList.Count - 1].Add(loadedData.items[j].key, loadedData.items[j].value);
                                }
                                //Check to make sure that all other entries after the initial English file have the correct Count as well as the exact same keys.
                                if (i > 0)
                                {
                                    if (!FilesAreEqual(localizedTextList[0], localizedTextList[localizedTextList.Count - 1]))
                                    {
                                        Debug.Log("Error: Localization File " + filePath + " can not be used due to inconsistencies with the base Localization File.");
                                    }
                                    else tempRegist.fileEntries.Add(m_FileRegistry.fileEntries[i]);
                                }
                                else if (i == 0)
                                    tempRegist.fileEntries.Add(m_FileRegistry.fileEntries[i]);

                                Debug.Log(filePath + " successfully loaded for localization. -- " + localizedTextList[localizedTextList.Count - 1].Count + " entries.");
                            }
                            catch
                            {
                                Debug.Log("Error: Localization File " + filePath + " found, but can not be read.");
                                m_Status = LocalizationStatus.fail;
                            }
                        }
                        else
                        {
                            Debug.Log("Error: Localization File " + filePath + " Not found");
                        }
                    }
                    if (localizedTextListCount == 0)
                    {
                        Debug.Log("Error: No localization text files could be found. Localization has failed");
                        m_Status = LocalizationStatus.fail;
                        return;
                    }
                    //modify the registy with the correct list of valid localization options.
                    m_FileRegistry = tempRegist;

                    m_Status = LocalizationStatus.success;
                }
                catch
                {
                    Debug.Log("Error: Registry File exists, but can not be read properly Localization has failed.");
                    m_Status = LocalizationStatus.fail;
                }
            }
            else
            {
                Debug.Log("Error: No Registry File exists. Localization has failed");
                m_Status = LocalizationStatus.fail;
            } 
        }

        public string GetLocalizedValue(string key)
        {
            string result = missingTextString;
            if (currentLocalizedText.ContainsKey(key))
            {
                result = currentLocalizedText[key];
            }

            return result;
        }

        private static bool FilesAreEqual(Dictionary<string, string> file1, Dictionary<string, string> file2)
        {
            if (file1.Count == file2.Count)
            {
                foreach (string key in file1.Keys)
                {
                    if (!file2.ContainsKey(key))
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        public void SetLocalizedText(int index)
        {
            if (index < localizedTextList.Count)
            {
                currentFont = fontList[index];
                currentLocalizedText = localizedTextList[index];
            }
            else
            {
                currentFont = fontList[0];
                currentLocalizedText = localizedTextList[0];
            }

            if (onLocalizationFileChanged != null)
                onLocalizationFileChanged();
        }

        public string GetLocalizedTextLanguage(int index)
        {
            return localizedTextList[index]["_Language"];
        }

        public Font GetCurrentFont()
        {
            return currentFont;
        }
    }


    public enum LocalizationStatus
    {
        validating = 0,
        success = 1,
        fail = 2
    }
}