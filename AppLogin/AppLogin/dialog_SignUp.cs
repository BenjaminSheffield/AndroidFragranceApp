using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AppLogin
{
    public class OnSignUpEventArgs : EventArgs
    {
        private string _FirstName;
        private string _Email;
        private string _Password;

        public string FirstName 
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        public string Email
        {
            get { return _Email; }
            set { _FirstName = value; }
        }

        public string Password
        {
            get { return _Password; }
            set { _Password = value; }
        }

        public OnSignUpEventArgs(string firstName, string email, string password) :base()
        {
            FirstName = firstName;
            Email = email;
            Password = password;
        }
    }

    class DialogSignUp : DialogFragment
    {
        private EditText _TxtFirstName;
        private EditText _TxtEmail;
        private EditText _TxtPassword;
        private Button _BtnSignUp;

        public event EventHandler<OnSignUpEventArgs> _OnSignUpComplete;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.dialog_sign_up, container, false);

            _TxtFirstName = view.FindViewById<EditText>(Resource.Id.txtFirstName);
            _TxtEmail = view.FindViewById<EditText>(Resource.Id.txtEmail);
            _TxtPassword = view.FindViewById<EditText>(Resource.Id.txtPassword);
            _BtnSignUp = view.FindViewById<Button>(Resource.Id.btnDialogEmail);

            _BtnSignUp.Click += _BtnSignUp_Click;

            return view;
        }

        void _BtnSignUp_Click(object sender, EventArgs e)
        {
            //User has clicked the signup button
            _OnSignUpComplete.Invoke(this, new OnSignUpEventArgs(_TxtFirstName.Text, _TxtEmail.Text, _TxtPassword.Text));
            Dismiss();
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //Sets the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation; //Set the animation
        }
    }
}