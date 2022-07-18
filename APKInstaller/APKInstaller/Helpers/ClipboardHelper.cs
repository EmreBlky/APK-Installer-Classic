using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows;
using Windows.Storage;
using System.IO;
using System.Collections.Specialized;
using System.Windows.Media.Imaging;

namespace APKInstaller.Helpers
{
    /// <summary>
    /// Class providing functionality to support generating and copying protocol activation URIs.
    /// </summary>
    public static class ClipboardHelper
    {
        /// <summary>
        /// ���Ƶ����а�
        /// </summary>
        /// <param name="text">����</param> 
        public static void CopyText(string text)
        {
            Clipboard.SetText(text);
        }

        /// <summary>
        /// ���Ƶ����а�
        /// </summary>
        /// <param name="image">ͼƬ</param> 
        public static void CopyBitmap(BitmapSource image)
        {
            Clipboard.SetImage(image);
        }

        /// <summary>
        /// ���ƻ�����ļ������а�
        /// </summary>
        /// <param name="filePath">�ļ�·������</param>
        /// <remarks>��ռ��а�</remarks>
        public static void SetFileDrop(string filePath)
        {
            if (filePath == null) return;
            SetFileDropList(new[] { filePath });
        }
        /// <summary>
        /// ���ƻ�����ļ������а�
        /// </summary>
        /// <param name="files">�ļ�·������</param>
        /// <remarks>��ռ��а�</remarks>
        public static void SetFileDropList(string[] files)
        {
            Clipboard.Clear();//��ռ��а� 
            StringCollection strcoll = new StringCollection();
            foreach (var file in files)
            {
                strcoll.Add(file);
            }
            Clipboard.SetFileDropList(strcoll);
        }

        /// <summary>
        /// ���ƻ�����ļ������а�
        /// </summary>
        /// <param name="filePath">�ļ�·������</param>
        /// <param name="cut">true:���У�false:����</param>
        public static void CopyFile(string filePath, bool cut = false)
        {
            if (filePath == null) return;
            CopyFileList(new[] { filePath }, cut);
        }
        /// <summary>
        /// ���ƻ�����ļ������а�
        /// </summary>
        /// <param name="files">�ļ�·������</param>
        /// <param name="cut">true:���У�false:����</param>
        public static void CopyFileList(string[] files, bool cut = false)
        {
            if (files == null) return;
            IDataObject data = new DataObject(DataFormats.FileDrop, files);
            MemoryStream memo = new MemoryStream(4);
            byte[] bytes = new byte[] { (byte)(cut ? 2 : 5), 0, 0, 0 };
            memo.Write(bytes, 0, bytes.Length);
            data.SetData("PreferredDropEffect", memo);
            Clipboard.SetDataObject(data, false);
        }

        /// <summary>
        /// ��ȡ�������е��ļ��б�������
        /// </summary>
        /// <returns>System.Collections.List<string>���ؼ��а����ļ�·������</returns>
        public static List<string> GetClipboardList()
        {
            List<string> clipboardList = new List<string>();
            StringCollection sc = Clipboard.GetFileDropList();
            foreach (var listFileName in sc)
            {
                clipboardList.Add(listFileName);
            }
            return clipboardList;
        }
    }
}
