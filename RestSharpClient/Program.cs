// See https://aka.ms/new-console-template for more information
using RestSharpClient.RestServices;
using System.Threading.Channels;

//Console.WriteLine("Hello, babe!");
//Console.WriteLine("Please enter your phonenumber:");
//var phonenumber = Console.ReadLine().Trim();

RestServices services = new();
//await services.GetPhonenumber(phonenumber);

//Thread.Sleep(1000);

Console.WriteLine("Please enter the code:");
var code = Console.ReadLine();

var token = services.GetSmsCode("09023007950", code).Result;


Console.WriteLine($"Your access token is: {token} ");

;
var users = services.GetAllUsers(token).Result;
;
users.ForEach(u => Console.WriteLine(u.Firstname + Environment.NewLine));
