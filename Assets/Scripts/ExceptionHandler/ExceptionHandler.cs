using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExceptionHandler
{
    public class Test: System.Exception
    {
        public string message;
    }

    public string title;
    public string content;

    public ExceptionHandler(string title, string content)
    {
        this.title = title;
        this.content = content;
    }
}
