﻿using Bugsnag.Maui;

namespace SampleApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
