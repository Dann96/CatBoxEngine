using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    /// <summary>
    /// Localization data.
    /// </summary>
    [System.Serializable]
    public class LocalizationData
    {
        public Font font;
        /// <summary>
        /// The collection of localization items.
        /// </summary>
        public LocalizationItem[] items;
    }
    /// <summary>
    /// Localization item.
    /// </summary>
    [System.Serializable]
    public class LocalizationItem
    {
        /// <summary>
        /// The key.
        /// </summary>
        public string key;
        /// <summary>
        /// The value.
        /// </summary>
        [Multiline]
        public string value;
    }
}