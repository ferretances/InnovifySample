﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace InnovifySample.Droid
{
  /*
   * Contact Form View.
   */
  public class Contact : BaseFragment
  {
    protected override int LayoutRes => Resource.Layout.Contact;
    public override void OnViewCreated(View view, Bundle savedInstanceState)
    {
      base.OnViewCreated(view, savedInstanceState);

      var name     = view.FindViewById<EditText>(Resource.Id.name);
      var email    = view.FindViewById<EditText>(Resource.Id.email);
      var phone    = view.FindViewById<EditText>(Resource.Id.phone);
      var website  = view.FindViewById<EditText>(Resource.Id.website);
      var position = view.FindViewById<EditText>(Resource.Id.position);
      var message  = view.FindViewById<EditText>(Resource.Id.message);
      var contact  = view.FindViewById<Button>(Resource.Id.contactUs);

      Observable.CombineLatest(
        name    .RxTextChanged().StartWith(string.Empty),
        email   .RxTextChanged().StartWith(string.Empty),
        phone   .RxTextChanged().StartWith(string.Empty),
        website .RxTextChanged().StartWith(string.Empty),
        position.RxTextChanged().StartWith(string.Empty),
        message .RxTextChanged().StartWith(string.Empty)
        , Tuple.Create)
      .SampleLatest(contact.RxClick())
       .Do(_ =>
         OnClean(name, email, phone, website, position, message))
      .SelectMany(_ =>
        OnValidate(
          Tuple.Create(name, _.Item1),
          Tuple.Create(email, _.Item2),
          Tuple.Create(phone, _.Item3),
          Tuple.Create(website, _.Item4),
          Tuple.Create(position, _.Item5),
          Tuple.Create(message, _.Item6))
     );

    }

    void OnClean(
     EditText name,
     EditText email,
     EditText phone,
     EditText website,
     EditText position,
     EditText message)
    {
      name.Error     =
      email.Error    =
      phone.Error    =
      website.Error  =
      position.Error =
      message.Error  = null;
    }

    IObservable<ContactInfo> OnValidate(
      Tuple<EditText, string> name,
      Tuple<EditText, string> email,
      Tuple<EditText, string> phone,
      Tuple<EditText, string> website,
      Tuple<EditText, string> position,
      Tuple<EditText, string> message) =>

        OnValidate(name)     ||
        OnValidate(email)    ||
        OnValidate(phone)    ||
        OnValidate(website)  ||
        OnValidate(position) ||
        OnValidate(message)
    
        ? Observable.Empty<ContactInfo>()
        : Observable.Return(new ContactInfo { 
          name     = name.Item2,
          email    = email.Item2,
          phone    = phone.Item2,
          website  = website.Item2,
          position = position.Item2,
          message  = message.Item2
        });

    bool OnValidate(Tuple<EditText, string> t) {
      var invalid = string.IsNullOrEmpty(t.Item2);
      if (invalid)
        t.Item1.Error = GetString(Resource.String.required);
      return invalid;
    }

  }

  /*
   *  Welcome or first displayd View.
   */
  public class Welcome : BaseFragment
	{
    protected override int LayoutRes => Resource.Layout.Welcome;

    public override void OnViewCreated(View view, Bundle savedInstanceState)
    {
      base.OnViewCreated(view, savedInstanceState);
      view.FindViewById<Button>(Resource.Id.ok)
          .RxClick()
          .Select(_ => Section.Contact)
          .Subscribe(Nav)
          .AddTo(Disposables);
    }

	} 

  /*
   *  Bye or last step View.
   */
  public class Bye : BaseFragment
  {
    protected override int LayoutRes => Resource.Layout.Bye;
    public override void OnViewCreated(View view, Bundle savedInstanceState)
    {
      base.OnViewCreated(view, savedInstanceState);
      view.FindViewById<Button>(Resource.Id.ok)
          .RxClick()
          .Select(_ => Section.Contact)
          .Subscribe(Nav)
          .AddTo(Disposables);
    }

  }
}
