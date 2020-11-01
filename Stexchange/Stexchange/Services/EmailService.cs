using MimeKit;
using MimeKit.Text;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Security;
using MailKit.Net.Smtp;
using System;

namespace Stexchange.Services
{
	public class EmailService : IDisposable
	{
		private readonly BlockingCollection<MimeMessage> queue = new BlockingCollection<MimeMessage>();

		private Thread workerThread;

		private IConfiguration Config { get; }
		private ILogger Log { get; }

		public EmailService(IConfiguration config, ILogger<EmailService> logger)
		{
			Config = config.GetSection("MailSettings");
			Log = logger;

			workerThread = new Thread(() =>
			{
				var task = Run();
				task.Wait();
			})
			{ Name = "EmailSender" };
			workerThread.Start();
		}

		private async Task Run()
		{
			MimeMessage message;
			while (!queue.IsAddingCompleted)
			{
				try
				{
					message = queue.Take();
				}
				catch (Exception e) when (e is ObjectDisposedException || e is InvalidOperationException || e is OperationCanceledException)
				{
					// Do nothing (queue is done)
					break;
				}

				Log.LogInformation($"Processing email message {message.Subject} at {DateTime.Now}");

				// send email
				using var smtp = new SmtpClient();
				Log.LogInformation($"after smtpclient {DateTime.Now}");
				await smtp.ConnectAsync(Config["Host"], Config.GetValue<int>("Port"), SecureSocketOptions.StartTls);
				Log.LogInformation($"after connect {DateTime.Now}");
				await smtp.AuthenticateAsync(Config["Email"], Config["Password"]);
				Log.LogInformation($"after authenticate {DateTime.Now}");
				await smtp.SendAsync(message);
				Log.LogInformation($"after send {DateTime.Now}");
				await smtp.DisconnectAsync(true);
				Log.LogInformation($"after disconnect {DateTime.Now}");

				Log.LogInformation($"Send email message {message.Subject} at {DateTime.Now}");
			}
			Log.LogInformation($"Exiting thread {workerThread.Name} at {DateTime.Now}");
		}

		/// <summary>
        /// Queues a new email message for this service.
        /// </summary>
        /// <param name="address">The email to send a message to.</param>
        /// <param name="body">The contents of the email.</param>
		public void QueueMessage(string address, string body)
		{
			// create email message
			var email = new MimeMessage();
			email.From.Add(new MailboxAddress("StexChange", Config["Email"]));
			email.To.Add(MailboxAddress.Parse(address));
			email.Subject = "E-mail Verificatie";
			email.Body = new TextPart(TextFormat.Plain) { Text = body };

			Log.LogInformation($"Queuing message for address {address}");
			queue.Add(email);
		}

		public void Dispose()
		{
			if (workerThread != null)
			{
				queue.CompleteAdding();
				workerThread.Join();
				workerThread = null;
			}
			queue.Dispose();
		}
	}
}