using System;
using Bugsnag.Maui;

namespace SampleApp;

public partial class MainPage : ContentPage
{
    private readonly IBugsnag bugsnag;

    public MainPage(IBugsnag bugsnag)
    {
        this.bugsnag = bugsnag;
        InitializeComponent();
    }

    private void OnBreadcrumbClicked(object? sender, EventArgs e)
    {
        this.bugsnag.LeaveBreadcrumb("Breadcrumb clicked");
    }

    private void OnErrorClicked(object? sender, EventArgs e)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            this.bugsnag.Notify(exception);
        }
    }

    private void OnDifferentErrorClicked(object? sender, EventArgs e)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception exception)
        {
            this.bugsnag.Notify(exception);
        }
    }

    private void OnCrashClicked(object? sender, EventArgs e)
    {
        throw new ExpectedCrashException("I wanted to crash!!!");
    }

    private void OnNestedErrorClicked(object? sender, EventArgs e)
    {
        try
        {
            OneLevelDeep();
        }
        catch (Exception exception)
        {
            this.bugsnag.Notify(exception);
            //App.Bugsnag.Notify(exception);
        }
    }

    private void OneLevelDeep()
    {
        TwoLevelsDeep();
    }

    private void TwoLevelsDeep()
    {
        DisplayAlert("Error", "Throw Exception", "OK");
        throw new Exception();
    }
}

public class ExpectedCrashException(string message) : Exception(message);
