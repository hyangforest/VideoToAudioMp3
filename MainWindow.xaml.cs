using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Documents;
using System.Linq;
using System.Windows.Input;
using System.IO;
using System;
using NAudio.Wave;

namespace VideoToAudioVer2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string videoFilePath = string.Empty;
        private string audioDirectory = string.Empty;    

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 사용자가 audio 파일을 저장할 디렉터리를 선택한다.
        /// </summary>
        /// <returns>선택한 폴더명</returns>
        private string SelectDirectory() 
        {
            string selectDirectory = string.Empty;

            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    selectDirectory = folderBrowserDialog.SelectedPath;
                    audioDirectory = folderBrowserDialog.SelectedPath;
                }
            }

            return selectDirectory;
        }

        /// <summary>
        /// NAudio NuGet 패키지를 이용하여 Video 파일을 Audio 파일로 변환하기
        /// </summary>
        /// <param name="videoFilePath">Video 파일 경로</param>
        /// <param name="outputMp3Path">저장할 .mp3 경로</param>
        /// <returns></returns>
        private bool ExtractAudioFromVideo(string videoFilePath, string outputMp3Path)
        {
            try
            {
                using (var reader = new MediaFoundationReader(videoFilePath))
                {
                    // NAudio를 사용하여 오디오 스트림을 .mp3 파일로 저장
                    WaveFileWriter.CreateWaveFile(outputMp3Path, reader);
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private void ChangeButtonColor(Border borderElement, string hasValue, string defaultColorCode, string hasColorCode) 
        {
            Color color;

            if (!string.IsNullOrEmpty(hasValue))
            {
                color = (Color)ColorConverter.ConvertFromString(hasColorCode);
                borderElement.Background = new SolidColorBrush(color);
            }
            else
            {
                color = (Color)ColorConverter.ConvertFromString(defaultColorCode);
                borderElement.Background = new SolidColorBrush(color);
            }
        }

        /// <summary>
        /// 사용자가 Audio 파일로 변환할 Video 파일의 경로를 가져온다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindVideo_Click(object sender, RoutedEventArgs e)
        {
            videoFilePath = string.Empty;

            // 폴더 선택 대화 상자를 열고, 사용자가 폴더를 선택하도록 합니다.
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "동영상 파일 열기",
                Multiselect = false, // 단일 파일 선택
                Filter = "Video Files|*.mp4;*.avi;*.mkv;*.wmv;*.flv;*.mov;*.mpeg|All Files|*.*",
                FilterIndex = 1
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // 선택된 비디오 파일들의 경로를 가져옵니다.
                string[] selectedFilePaths = openFileDialog.FileNames;

                // 선택된 파일들을 출력합니다.
                if (selectedFilePaths.Length > 0)
                {
                    string fileListString = string.Join("\n", selectedFilePaths);

                    // 파일 저장하기
                    videoFilePath = fileListString;
                }
                else
                {
                    System.Windows.MessageBox.Show("파일 정보를 가져올 수 없습니다.", "Video 파일 열기");
                }
            }

            // 버튼 색깔 바꾸기
            Border? borderElement = btnFindVideo.Template.FindName("borderBtnFindVideo", btnFindVideo) as Border;

            if (borderElement != null)
            {
                ChangeButtonColor(borderElement, videoFilePath, "#FFF2BE80", "#9ACD32");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(videoFilePath))
            {
                System.Windows.MessageBox.Show("먼저 변환할 Video 파일을 선택해주세요.", "Video 파일 열기");
            }
            else
            {
                if (!string.IsNullOrEmpty(SelectDirectory()))
                {
                    // 커서 Wait 처리
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;


                    if (string.IsNullOrEmpty(audioDirectory))
                    {
                        System.Windows.MessageBox.Show("변환할 파일을 저장할 폴더를 선택해주세요.", "Audio 파일 변환하기");
                    }
                    else
                    {
                        string videoFileName = videoFilePath.Split('\\')[videoFilePath.Split('\\').Length - 1].Split('.')[0]; // 비디오 파일명
                        string outputAudioFilePath = string.Format("{0}\\{1}_audio_{2}.mp3", audioDirectory, videoFileName, DateTime.Now.ToString("yyyyMMddHHmmss")); // .mp3 파일 경로


                        if (ExtractAudioFromVideo(videoFilePath, outputAudioFilePath))
                        {
                            // 버튼 색깔 바꾸기
                            Border? borderFind = btnFindVideo.Template.FindName("borderBtnFindVideo", btnFindVideo) as Border;
                            Border? borderSave = btnSave.Template.FindName("borderBtnSave", btnSave) as Border;

                            if (borderSave != null)
                            {
                                ChangeButtonColor(borderSave, audioDirectory, "#F29580", "#32B8CD");
                            }

                            System.Windows.Clipboard.SetText(audioDirectory);

                            MessageBoxResult result = System.Windows.MessageBox.Show(string.Format("파일 변환이 완료되었습니다.\n저장 경로는 {0}입니다.\n경로 복사되었습니다.\n프로그램을 초기 상태로 되돌리시겠습니까?", audioDirectory), "Audio 파일 변환하기", MessageBoxButton.OKCancel);

                            if (result == MessageBoxResult.OK)
                            {
                                videoFilePath = string.Empty;
                                audioDirectory = string.Empty;

                                if (borderFind != null)
                                {
                                    ChangeButtonColor(borderFind, videoFilePath, "#FFF2BE80", "#9ACD32");
                                }

                                if (borderSave != null)
                                {
                                    ChangeButtonColor(borderSave, audioDirectory, "#F29580", "#32B8CD");
                                }
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("파일 변환이 실패하였습니다.", "Audio 파일 변환하기");
                        }
                    }

                    // 커서 원래대로 복원
                    Mouse.OverrideCursor = null;
                }
            }
        }

        /// <summary>
        /// 푸터 프로그램 문의 이메일 클릭 시 주소 복사하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink hyperlink = (Hyperlink)sender;

            System.Windows.Clipboard.SetText(hyperlink.Inlines.OfType<Run>().FirstOrDefault()?.Text);

            System.Windows.MessageBox.Show("이메일 주소가 복사되었습니다.", "알림");
        }
    }
}
