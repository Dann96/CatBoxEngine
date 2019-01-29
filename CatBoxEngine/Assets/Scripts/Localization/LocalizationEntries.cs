using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    [System.Serializable]
    public class LocalizationFileRegistry
    {
        public List<LocalizationFileEntry> fileEntries;

        public LocalizationFileRegistry() { }

        public LocalizationFileRegistry(List<LocalizationFileEntry> _FileEntries)
        {
            fileEntries = _FileEntries;
        }
    }

    [System.Serializable]
    public class LocalizationFileEntry
    {
        public string fileName;
        public string languageName;
    }
}