using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using dev_framework.Manager.Model;
using dev_framework.Message.Model;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;

namespace dev_framework.Manager
{
    public enum ESerilogType { Console, File }

    /// <summary>
    /// Manages the configuration and logging using Serilog.
    /// </summary>
    public class SerilogManager
    {
        private string? _filename { get { return $"{_path}/{DateTime.Now.ToString("yyyy_MM_dd")}_{_name}.log"; } }
        private readonly ESerilogType _eSerilogType;
        private readonly string _path;
        private readonly string _name;
        private readonly string _discordUrl;

        /// <summary>
        /// Initializes a new instance of the SerilogManager class.
        /// </summary>
        /// <param name="eSerilogType">The type of Serilog logger to use.</param>
        /// <param name="name">The name of the logger.</param>
        /// <param name="path">The path where log files will be stored.</param>
        /// <param name="discordUrl">The Discord webhook URL for error notifications.</param>
        public SerilogManager(ESerilogType eSerilogType, string? name, string? path, string discordUrl)
        {
            _eSerilogType = eSerilogType;
            _name = name;
            _path = path;
            _discordUrl = discordUrl;
        }

        /// <summary>
        /// Gets the Serilog logger based on the configured type.
        /// </summary>
        /// <returns>The Serilog logger.</returns>
        private Logger GetLogger()
        {
            if (_eSerilogType == ESerilogType.File && !Directory.Exists(_path))
                Directory.CreateDirectory(_path);

            switch (_eSerilogType)
            {
                case ESerilogType.File:
                    return new LoggerConfiguration().WriteTo.File(_filename).CreateLogger();
                case ESerilogType.Console:
                default:
                    return new LoggerConfiguration().WriteTo.Console().CreateLogger();
            }
        }

        /// <summary>
        /// Logs the start of a method execution.
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="parameters">The method parameters.</param>
        /// <returns>The start time of the method execution.</returns>
        public DateTime Debut(string methodName, params object[] parameters)
        {
            StringBuilder log = new StringBuilder();
            log.Append("[DEBUT]");
            log.AppendFormat(" : [{0}]", methodName);

#if DEBUG
            if (parameters != null && parameters.Any())
            {
                var serialized = JsonConvert.SerializeObject(parameters, Formatting.Indented);
                log.AppendLine(serialized);
            }
#endif
            GetLogger().Information(log.ToString());
            return DateTime.Now;
        }

        /// <summary>
        /// Logs the end of a method execution with a return value.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="returnValue">The return value.</param>
        /// <param name="debut">The start time of the method execution.</param>
        public void Fin<T>(string methodName, T returnValue, DateTime debut) where T : class
        {
            StringBuilder log = new StringBuilder();
            log.Append("[Fin]");
            log.Append($" : [{methodName}] --- Duration : {GetDuration(debut, DateTime.Now)} seconde(s)");

#if DEBUG
            if (returnValue != null)
            {
                var serialized = JsonConvert.SerializeObject(returnValue, Formatting.Indented);
                log.AppendLine(serialized);
            }
#endif

            GetLogger().Information(log.ToString());
        }

        /// <summary>
        /// Logs the end of a method execution with a return value of type IEnumerable.
        /// </summary>
        /// <typeparam name="T">The type of the return value.</typeparam>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="returnValue">The return value.</param>
        /// <param name="debut">The start time of the method execution.</param>
        public void Fin<T>(string methodName, IEnumerable<T> returnValue, DateTime debut) where T : class
        {
            StringBuilder log = new StringBuilder();
            log.Append("[Fin]");
            log.Append($" : [{methodName}] --- Duration : {GetDuration(debut, DateTime.Now)} seconde(s)");

#if DEBUG
            if (returnValue != null)
            {
                var serialized = JsonConvert.SerializeObject(returnValue, Formatting.Indented);
                log.AppendLine(serialized);
            }
#endif

            GetLogger().Information(log.ToString());
        }
        public void Fin<T>(string methodName, DateTime debut) where T : class
        {
            StringBuilder log = new StringBuilder();
            log.Append("[Fin]");
            log.Append($" : [{methodName}] --- Duration : {GetDuration(debut, DateTime.Now)} seconde(s)");
            GetLogger().Information(log.ToString());
        }


        /// <summary>
        /// Logs an error with the specified method name, exception, and ID.
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="ex">The exception.</param>
        /// <param name="id">The ID.</param>
        /// <returns>The database message.</returns>
        public void ErrorById(string methodName, Exception ex, int id, bool throwEx = false)
        {
            StringBuilder log = new StringBuilder();
            log.Append("[Error]");
            log.Append($" : [{methodName}] -- ID : {id}");
            log.AppendLine($"Exception : {JsonConvert.SerializeObject(ex, Formatting.Indented)}");
            GetLogger().Error(log.ToString());

            if (!string.IsNullOrEmpty(_discordUrl))
                NotificationManager.Current.PublishDiscordWebHook(ex, id, _discordUrl);

            if (throwEx)
                throw ex;
        }
        public void Error(string methodName, Exception ex, bool throwEx = false)
        {
            StringBuilder log = new StringBuilder();
            log.Append("[Error]");
            log.AppendLine($"Exception : {JsonConvert.SerializeObject(ex, Formatting.Indented)}");
            GetLogger().Error(log.ToString());

            if (!string.IsNullOrEmpty(_discordUrl))
                NotificationManager.Current.PublishDiscordWebHook(ex, null, _discordUrl);

            if (throwEx)
                throw ex;
        }

        /// <summary>
        /// Logs an error with the specified method name and exception.
        /// </summary>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="ex">The exception.</param>
        /// <returns>The database message.</returns>
        public void Error(string methodName, Exception ex)
        {
            Error<object>(methodName, ex, null);
        }

        /// <summary>
        /// Logs an error with the specified method name, exception, and current object.
        /// </summary>
        /// <typeparam name="T">The type of the current object.</typeparam>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="ex">The exception.</param>
        /// <param name="currentObj">The current object.</param>
        /// <returns>The database message.</returns>
        public void Error<T>(string methodName, Exception ex, T currentObj, bool throwEx = false) where T : class
        {
            StringBuilder log = new StringBuilder();
            log.Append("[Error]");
            log.AppendFormat(" : [{0}]", methodName);
            if (currentObj != null)
            {
                var serialized = JsonConvert.SerializeObject(currentObj, Formatting.Indented);
                log.AppendLine(serialized);
            }
            log.AppendLine($"Exception : {JsonConvert.SerializeObject(ex, Formatting.Indented)}");
            GetLogger().Error(log.ToString());

            if (!string.IsNullOrEmpty(_discordUrl))
                NotificationManager.Current.PublishDiscordWebHook(ex, currentObj, _discordUrl);

            if (throwEx)
                throw ex;
        }

        /// <summary>
        /// Logs an error with the specified method name, exception, and current objects.
        /// </summary>
        /// <typeparam name="T">The type of the current objects.</typeparam>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="ex">The exception.</param>
        /// <param name="currentObj">The current objects.</param>
        /// <returns>The database message.</returns>
        public void Error<T>(string methodName, Exception ex, IEnumerable<T>? currentObj = null, bool throwEx = false) where T : class
        {
            StringBuilder log = new StringBuilder();
            log.Append("[Error]");
            log.AppendFormat(" : [{0}]", methodName);
            if (currentObj != null)
            {
                var serialized = JsonConvert.SerializeObject(currentObj, Formatting.Indented);
                log.AppendLine(serialized);
            }
            log.AppendLine($"Exception : {JsonConvert.SerializeObject(ex, Formatting.Indented)}");
            GetLogger().Error(log.ToString());

            if (!string.IsNullOrEmpty(_discordUrl))
                NotificationManager.Current.PublishDiscordWebHook(ex, currentObj, _discordUrl);

            if (throwEx)
                throw ex;
        }

        /// <summary>
        /// Calculates the duration in seconds between two DateTime values.
        /// </summary>
        /// <param name="start">The start DateTime.</param>
        /// <param name="end">The end DateTime.</param>
        /// <returns>The duration in seconds.</returns>
        private int GetDuration(DateTime start, DateTime end)
        {
            TimeSpan duration = end - start;
            return (int)duration.TotalSeconds;
        }

        /// <summary>
        /// Gets the name of the current method.
        /// </summary>
        /// <returns>The name of the current method.</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            return sf.GetMethod().Name;
        }
    }
}
