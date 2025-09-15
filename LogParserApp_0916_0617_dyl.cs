// 代码生成时间: 2025-09-16 06:17:04
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace LogParserApp
{
    // 主窗体
    public class LogParserApp : ContentPage
    {
        private Entry logFilePathEntry;
        private Button parseButton;
        private ListView logEntriesListView;
        private List<LogEntry> logEntries;

        public LogParserApp()
        {
            // 初始化UI组件
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // 文件路径输入框
            logFilePathEntry = new Entry
            {
                Placeholder = "Enter log file path"
            };

            // 解析按钮
            parseButton = new Button
            {
                Text = "Parse Log"
            };
            parseButton.Clicked += OnParseClicked;

            // 日志条目列表视图
            logEntriesListView = new ListView
            {
                RowHeight = 50
            };

            // 布局设置
            Content = new StackLayout
            {
                Children =
                {
                    logFilePathEntry,
                    parseButton,
                    logEntriesListView
                }
            };
        }

        private async void OnParseClicked(object sender, EventArgs e)
        {
            string logFilePath = logFilePathEntry.Text;
            try
            {
                if (!File.Exists(logFilePath))
                {
                    await DisplayAlert("Error", "Log file not found.", "OK");
                    return;
                }

                logEntries = ParseLogEntries(logFilePath);
                logEntriesListView.ItemsSource = logEntries;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // 解析日志文件
        private List<LogEntry> ParseLogEntries(string logFilePath)
        {
            var logEntries = new List<LogEntry>();
            string pattern = @"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}).*?(\w+).*?(.*)";

            using (StreamReader reader = new StreamReader(logFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Match match = Regex.Match(line, pattern);
                    if (match.Success)
                    {
                        logEntries.Add(new LogEntry
                        {
                            Timestamp = DateTime.Parse(match.Groups[1].Value),
                            LogLevel = match.Groups[2].Value,
                            Message = match.Groups[3].Value
                        });
                    }
                }
            }

            return logEntries;
        }

        // 日志条目类
        private class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public string LogLevel { get; set; }
            public string Message { get; set; }
        }
    }
}
