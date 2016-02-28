# Xamarin.Forms.Essentials

This repository houses a few components and functions to make your life in Xamarin.Forms that little bit easier. 

### State Container [all platforms]
StateContainer is a visual component which allows Mvvm bindings to Enums. This allows really clean viewmodels to be written, and encourages better readability in the Viewmodel and the View.
See [this blogpost](http://blog.xdelivered.com/binding-state-mvvm-xamarin-forms) for more details.

```C#
 <controls:StateContainer State="{Binding LoggedInState}" BackgroundColor="White">
   <controls:StateCondition Is="Idle">
     <!-- Logging in entry -->
     <StackLayout Orientation="Vertical" WidthRequest="250" HeightRequest="150" HorizontalOptions="CenterAndExpand" VerticalOptions="CenterAndExpand">
       <StackLayout Orientation="Vertical">
         <Entry Placeholder="Username" Text="{Binding UserName}" />
         <Entry Placeholder="Password" IsPassword="True" Text="{Binding Password}" />
       </StackLayout>
       <Button Text="Login" Command="{Binding LoginCommand}" />
     </StackLayout>
   </controls:StateCondition>
   <controls:StateCondition Is="LoggingIn">
     <!-- Logging in -->
     <ActivityIndicator IsRunning="True" />
   </controls:StateCondition>
   <controls:StateCondition Is="LoggedIn">
     <!-- Logged in successfully -->
     <StackLayout Orientation="Vertical" HorizontalOptions="CenterAndExpand" VerticalOptions="CenterAndExpand">
       <Image Source="https://cdn3.iconfinder.com/data/icons/10con/512/checkmark_tick-128.png" WidthRequest="35" HeightRequest="35" />
       <Label Text="Logged In" />
     </StackLayout>
   </controls:StateCondition>
   <controls:StateCondition Is="Problem">
     <!-- Login Problem -->
     <Label Text="Sorry there was a problem logging in" TextColor="Red"></Label>
   </controls:StateCondition>
 </controls:StateContainer>
```
  
### GradientLayer [iOS only for now]
Allows gradient backgrounds to be created.
Samples, droid + win implementation coming soon
  
### BlurryPanel [iOS only for now]
Allows a blurry panel to be used. This is really on an iOS UI pattern, so can only be used there.
  
### Circle [All platforms]
Allows circles to be used in Forms. Set stokethickness, color & fillcolor.
Sample coming soon
  
  
