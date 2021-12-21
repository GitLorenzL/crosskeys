using System;
public class ErrorRate  {
    private const int MAX = 500;
    private const char DEL = '-';
    private int delTimes = 0;

    int[] resTail = new int[MAX];
    int[] resLen = new int[MAX];
    int resIndex = 0;
    int[] accessed = new int[MAX];

    public ErrorRate(int delTimes) {this.delTimes = delTimes;}

    private void LIS_update(int tail) {
        bool append = true;
        for (int i = 0; i < resIndex; ++i) {
            if (tail >= resTail[i]) {
                resTail[i] = tail;
                ++resLen[i];
                append = false;
            }
        }
        if (append) {
            resLen[resIndex] = 1;
            resTail[resIndex] = tail;
            ++resIndex;
        }
    }

    private int LIS(int[] arr, int len) {
        resIndex = 0;
        for (int i = 0; i < len; ++i)
            if (arr[i] <= len)
                LIS_update(arr[i]);
        int maxLen = resLen[0];
        for (int i = 0; i < len; ++i)
            maxLen = maxLen > resLen[i] ? maxLen : resLen[i];
        return maxLen;
    }

    private int search(string str, char trg) {
        for (int i = 0; i < str.Length; ++i) {
            if (str[i] == trg && accessed[i] == 0) {
                accessed[i] = 1;
                return i;
            }
        }
        return str.Length + 1;
    }

    private string preprocess(string str) {
        char[] res = new char[str.Length];
        int cnt = 0;
        for (int i = 0; i < str.Length; ++i) {
            if (str[i] == DEL) {
                if (cnt > 0) {
                    --cnt;
                    res[cnt] = '\0';
                }
            } else res[cnt++] = str[i];
        }
        
        string returnVal = new string(res);
        return returnVal.Substring(0, cnt);
    }

    private void eliminateSpacesOnTail(string str) {
        
    }

    public double TER(string std_str, string test_str) {
        if (test_str.Length < std_str.Length)
            std_str = std_str.Substring(0, test_str.Length);

        accessed = new int[MAX];
        resTail = new int[MAX];
        resLen = new int[MAX];
        int stdlen = std_str.Length;
        int cut = 0;
        for (int i = test_str.Length - 1; i >= 0; --i) {
            if (test_str[i] == ' ') {
                ++cut;
            } else break;
        }
        test_str = test_str.Substring(0, test_str.Length - cut);

        string processed = preprocess(test_str);
 
        int len1 = std_str.Length;
        int len2 = processed.Length;
        int[,] dif = new int[len1 + 1, len2 + 1];
        for (int a = 0; a <= len1; a++)
            dif[a, 0] = a;
        for (int a = 0; a <= len2; a++)
            dif[0, a] = a; 
        int temp;
        for (int i = 1; i <= len1; i++)
        for (int j = 1; j <= len2; j++) {
            temp = std_str[i - 1] == processed[j - 1] ? 0 : 1;
            dif[i, j] = Math.Min(Math.Min(dif[i - 1, j - 1] + temp, dif[i, j - 1] + 1), dif[i - 1, j] + 1);
        }
        return ((double)dif[len1, len2] + this.delTimes) / Math.Max(std_str.Length, processed.Length);
    }

    public double NCER(string std_str, string test_str) {
        if (test_str.Length < std_str.Length)
            std_str = std_str.Substring(0, test_str.Length);
        
        accessed = new int[MAX];
        resTail = new int[MAX];
        resLen = new int[MAX];
        int stdlen = std_str.Length;
        int cut = 0;
        for (int i = test_str.Length - 1; i >= 0; --i) {
            if (test_str[i] == ' ') {
                ++cut;
            } else break;
        }
        test_str = test_str.Substring(0, test_str.Length - cut);

        string processed = preprocess(test_str);
 
        int len1 = std_str.Length;
        int len2 = processed.Length;
        int[,] dif = new int[len1 + 1, len2 + 1];
        for (int a = 0; a <= len1; a++)
            dif[a, 0] = a;
        for (int a = 0; a <= len2; a++)
            dif[0, a] = a; 
        int temp;
        for (int i = 1; i <= len1; i++)
        for (int j = 1; j <= len2; j++) {
            temp = std_str[i - 1] == processed[j - 1] ? 0 : 1;
            dif[i, j] = Math.Min(Math.Min(dif[i - 1, j - 1] + temp, dif[i, j - 1] + 1), dif[i - 1, j] + 1);
        }
        return (double)dif[len1, len2] / Math.Max(std_str.Length, processed.Length);
    }
}