using Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDialogBox : UIWindow, IPointerClickHandler
{

    public Text Tstr;

    List<string> strList = new List<string>();
    int now = 0;
    int max = 0;

    public void Init(string str)
    {
        str = str.Replace("XXX", User.Instance.CurrentCharacter.Name);

        int startIndex = 0;
        int targetLen = 30;
        int totalLen = str.Length;

        char[] sentenceEnd = { '。', '！', '？' };
        char comma = '，';

        while (startIndex < totalLen)
        {
            // 先跳过开头连续逗号，防止片段以逗号起始
            while (startIndex < totalLen && str[startIndex] == comma)
            {
                startIndex++;
            }
            if (startIndex >= totalLen) break;

            int endIndex = startIndex + targetLen;
            if (endIndex >= totalLen)
            {
                string seg = str.Substring(startIndex);
                strList.Add(seg);
                break;
            }

            int splitPos = endIndex;
            bool findPunct = false;

            // 优先找句末标点 。！？
            while (splitPos > startIndex)
            {
                if (sentenceEnd.Contains(str[splitPos]))
                {
                    splitPos += 1;
                    findPunct = true;
                    break;
                }
                splitPos--;
            }

            // 没找到句末标点，再找逗号
            if (!findPunct)
            {
                splitPos = endIndex;
                while (splitPos > startIndex)
                {
                    if (str[splitPos] == comma)
                    {
                        // 逗号作为分割点，逗号舍弃，不放进片段
                        findPunct = true;
                        break;
                    }
                    splitPos--;
                }
            }

            // 无合适标点兜底，直接截断
            if (!findPunct || splitPos <= startIndex)
            {
                splitPos = endIndex;
            }

            string piece = str.Substring(startIndex, splitPos - startIndex);
            strList.Add(piece);
            startIndex = splitPos;
        }
        this.max = strList.Count;
        if (this.max > 0)
            this.Tstr.text = strList[0];
        else
            Close();
        this.now = 1;
    }

    //点击切换下一句
    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.now >= this.max)
        {
            Close();
            return;
        }

        this.Tstr.text = strList[this.now];
        this.now++;
    }
}
