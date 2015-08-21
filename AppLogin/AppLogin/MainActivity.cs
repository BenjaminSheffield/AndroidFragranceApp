using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AppLogin
{
    [Activity(Label = "AppLogin", MainLauncher = true, Icon = "@drawable/icon_bottle")]
    public class MainActivity : Activity
    {
        private Button _BtnSignUp;
        private ProgressBar _ProgressBar;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _BtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            _ProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);

            _BtnSignUp.Click += (object sender, EventArgs args) =>
            {
                //Pull up dialog
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                DialogSignUp signUpDialog = new DialogSignUp();
                signUpDialog.Show(transaction, "dialog fragment");

                signUpDialog._OnSignUpComplete += signUpDialog__OnSignUpComplete;
            };
        }

        void signUpDialog__OnSignUpComplete(object sender, OnSignUpEventArgs e)
        {
            _ProgressBar.Visibility = ViewStates.Visible;
            Thread thread = new Thread(ActLikeARequest);
            thread.Start();
           }

        private void ActLikeARequest()
        {
            Thread.Sleep(3000);
            RunOnUiThread(() => { _ProgressBar.Visibility = ViewStates.Invisible; });
        }


    }
}

