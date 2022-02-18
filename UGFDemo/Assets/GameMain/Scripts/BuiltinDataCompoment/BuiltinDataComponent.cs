using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework;

namespace UGFDemo
{
    public class BuiltinDataComponent : GameFrameworkComponent
    {
        [SerializeField]
        private TextAsset m_BuildInfoTextAsset = null;

        [SerializeField]
        private TextAsset m_DefaultDictionaryTextAsset = null;


        private BuildInfo m_BuildInfo = null;

        public BuildInfo BuildInfo
        {
            get
            {
                return m_BuildInfo;
            }
        }

        public void InitBuildInfo()
        {
            if (m_BuildInfoTextAsset == null || string.IsNullOrEmpty(m_BuildInfoTextAsset.text))
            {
                Log.Info("Build info can not be found or empty.");
                return;
            }
            m_BuildInfo = Utility.Json.ToObject<BuildInfo>(m_BuildInfoTextAsset.text);
            if(m_BuildInfo == null)
            {
                Log.Warning("Parse build info failure.");
                return;
            }
        }

        
    }

}
