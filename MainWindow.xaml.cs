using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VideoToAudioVer2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string videoFilePath = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
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
            var openFileDialog = new OpenFileDialog
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
                    MessageBox.Show("파일 정보를 가져올 수 없습니다.", "Video 파일 열기");
                }
            }


            // 버튼 색깔 바꾸기
            Border? borderElement = btnFindVideo.Template.FindName("borderBtnFindVideo", btnFindVideo) as Border;

            if (borderElement != null)
            {
                Color color;

                if (!string.IsNullOrEmpty(videoFilePath))
                {
                    color = (Color)ColorConverter.ConvertFromString("#9ACD32");
                    borderElement.Background = new SolidColorBrush(color);
                }
                else
                {
                    color = (Color)ColorConverter.ConvertFromString("#FFF2BE80");
                    borderElement.Background = new SolidColorBrush(color);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
