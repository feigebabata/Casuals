
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

public class TextFormatter
{
    // 匹配章节名的正则表达式（支持"第X章"格式，X为汉字或数字序数词）
    private static readonly Regex ChapterRegex = new Regex(
        @"^第[零一二三四五六七八九十百千]+章\s*$", 
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    // 中文结束标点集合
    private static readonly HashSet<char> ChineseEndPunctuations = new HashSet<char> {
        '。', '！', '？', '；', '…', '》', '】', '〕', '〕'
    };

    /// <summary>
    /// 修复文本中的不正确换行
    /// </summary>
    /// <param name="inputText">原始输入文本</param>
    /// <returns>处理后的文本行列表</returns>
    public static List<string> FixIncorrectLineBreaks(string inputText)
    {
        // 分割文本为行（支持所有换行符格式）
        string[] rawLines = inputText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        List<string> result = new List<string>();
        StringBuilder currentParagraph = new StringBuilder();  // 当前正在构建的段落
        int? currentIndent = null;  // 当前段落的缩进量（全角空格数）

        foreach (string line in rawLines)
        {
            // 处理章节名行
            if (IsChapterLine(line))
            {
                FinalizeCurrentParagraph(result, currentParagraph, ref currentIndent);
                result.Add(line.TrimEnd());  // 保留章节名行，去除尾部可能空白
                continue;
            }

            // 计算当前行的缩进量（全角空格数）
            int lineIndent = CalculateFullWidthIndent(line);
            
            // 跳过纯空行（仅包含全角空格的行）
            if (lineIndent == -1) continue;

            // 处理段落合并逻辑
            ProcessLineForParagraph(ref currentParagraph, ref currentIndent, line, lineIndent, result);
        }

        // 处理最后一个未完成的段落
        FinalizeCurrentParagraph(result, currentParagraph, ref currentIndent);

        return result;
    }

    /// <summary>
    /// 判断是否为章节名行
    /// </summary>
    private static bool IsChapterLine(string line)
    {
        // 去除首尾空白后匹配章节名正则
        return ChapterRegex.IsMatch(line.Trim());
    }

    /// <summary>
    /// 计算行的全角空格缩进量（-1表示纯空行）
    /// </summary>
    private static int CalculateFullWidthIndent(string line)
    {
        int count = 0;
        foreach (char c in line)
        {
            if (c == '\u3000')  // 全角空格Unicode编码
                count++;
            else
                break;
        }
        // 纯空行返回-1，否则返回实际缩进量
        return count == line.Length ? -1 : count;
    }

    /// <summary>
    /// 处理单行文本，决定是否合并到当前段落
    /// </summary>
    private static void ProcessLineForParagraph(
        ref StringBuilder currentParagraph,
        ref int? currentIndent,
        string currentLine,
        int currentLineIndent,
        List<string> result)
    {
        // 初始化新段落
        if (currentParagraph.Length == 0)
        {
            currentParagraph.Append(currentLine);
            currentIndent = currentLineIndent;
            return;
        }

        // 缩进不一致：结束当前段落，开始新段落
        if (currentLineIndent != currentIndent)
        {
            result.Add(currentParagraph.ToString().TrimEnd());
            currentParagraph.Clear().Append(currentLine);
            currentIndent = currentLineIndent;
            return;
        }

        // 缩进一致：检查是否需要合并
        if (!EndsWithPunctuation(currentParagraph.ToString()))
        {
            // 未结束：合并当前行（去除重复缩进）
            currentParagraph.Append(currentLine.Substring(currentLineIndent));
        }
        else
        {
            // 已结束：结束当前段落，开始新段落
            result.Add(currentParagraph.ToString().TrimEnd());
            currentParagraph.Clear().Append(currentLine);
        }
    }

    /// <summary>
    /// 结束当前段落并添加到结果
    /// </summary>
    private static void FinalizeCurrentParagraph(
        List<string> result,
        StringBuilder currentParagraph,
        ref int? currentIndent)
    {
        if (currentParagraph.Length > 0)
        {
            result.Add(currentParagraph.ToString().TrimEnd());
            currentParagraph.Clear();
            currentIndent = null;
        }
    }

    /// <summary>
    /// 检查文本是否以中文结束标点结尾
    /// </summary>
    private static bool EndsWithPunctuation(string text)
    {
        if (string.IsNullOrEmpty(text)) return false;
        char lastChar = text[text.Length - 1];
        return ChineseEndPunctuations.Contains(lastChar);
    }
}