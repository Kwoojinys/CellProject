using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionChecker {

    string url = "";

    public IEnumerator Check()
    {
        WWW www = new WWW(url);
        yield return www;
        //인터넷 연결 에러가 없다면, 
        if (www.error == null)
        {
            int index = www.text.IndexOf("softwareVersion");
            Debug.Log(index);
            string versionText = www.text.Substring(index, 30);
            //플레이스토어에 올라간 APK의 버전을 가져온다.
            int softwareVersion = versionText.IndexOf(">");
            string playStoreVersion = versionText.Substring(softwareVersion + 1, Application.version.Length + 1);

            //버전이 같다면,
            if (playStoreVersion.Trim().Equals(Application.version))
            {
                //게임 씬으로 넘어간다.
                Debug.LogWarning("true : " + playStoreVersion + " : " + Application.version);

                //버전이 같다면, 앱을 넘어가도록 한다.
            }
            else
            {
                //버전이 다르므로, 마켓으로 보낸다.
                Debug.LogWarning("false : " + playStoreVersion + " : " + Application.version);

                //업데이트 팝업을 연결한다.
            }
        }
        else
        {
            //인터넷 연결 에러시
            Debug.LogWarning(www.error);
        }

    }

}
