﻿using AxoCover.Models.Data;
using AxoCover.Models.Events;
using Microsoft.Practices.Unity;
using System;

namespace AxoCover.Models
{
  public class MultiplexedTestRunner : Multiplexer<ITestRunner>, ITestRunner
  {
    public event TestExecutedEventHandler TestExecuted;
    public event TestLogAddedEventHandler TestLogAdded;
    public event EventHandler TestsFailed;
    public event TestFinishedEventHandler TestsFinished;
    public event EventHandler TestsStarted;

    public MultiplexedTestRunner(IUnityContainer container) : base(container)
    {
      foreach (var implementation in _implementations.Values)
      {
        implementation.TestExecuted += (o, e) => TestExecuted?.Invoke(this, e);
        implementation.TestLogAdded += (o, e) => TestLogAdded?.Invoke(this, e);
        implementation.TestsFailed += (o, e) => TestsFailed?.Invoke(this, e);
        implementation.TestsFinished += (o, e) => TestsFinished?.Invoke(this, e);
        implementation.TestsStarted += (o, e) => TestsStarted?.Invoke(this, e);
      }
    }

    public void RunTestsAsync(TestItem testItem, string testSettings = null)
    {
      _implementation.RunTestsAsync(testItem, testSettings);
    }
  }
}