using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace TracerLibrary
{
    public class Tracer : ITracer
    {
        private List<ThreadInfo> threads = new List<ThreadInfo> { };
        public static object locker = new object();

        public void StartTrace()
        {

            int threadId = Thread.CurrentThread.ManagedThreadId;
            ThreadInfo currentThread = null;
            Stopwatch threadTracer = new Stopwatch();
            lock (locker)
            {
                threads.ForEach(item => {
                    if (item._id == threadId)
                    {
                        currentThread = item;
                    }
                });
            }
            if (currentThread == null)
            {
                currentThread = new ThreadInfo(threadId, threadTracer);
                threads.Add(currentThread);
            }

            StackFrame frame = new StackFrame(1);
            MethodBase methodContext = frame.GetMethod();
            Stopwatch tracer = new Stopwatch();

            MethodInfo currentMethod = new MethodInfo(methodContext.DeclaringType.Name, methodContext.Name, tracer);
            if (currentThread.currMethod != null)
            {
                currentThread.currMethod.methods.Add(currentMethod);
                currentMethod.parent = currentThread.currMethod;
                currentThread.parent = currentThread.currMethod.parent;
                currentThread.currMethod = currentMethod;
            }
            else
            {
                currentThread.stackContext.Add(currentMethod);
                currentThread.currMethod = currentMethod;
                currentThread.parent = null;
                currentMethod.parent = null;
            }

            threadTracer.Start();
            tracer.Start();
        }

        public void StopTrace()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            ThreadInfo currentThread = null;
            lock (locker)
            {
                threads.ForEach(item => {
                    if (item._id == threadId)
                    {
                        currentThread = item;
                    }
                });
            }

            if (currentThread.currMethod.parent == null)
            {
                currentThread._tracer.Stop();
                currentThread.millisecinds = (int)currentThread._tracer.Elapsed.TotalMilliseconds;
            }

            currentThread.currMethod._tracer.Stop();
            currentThread.currMethod.millisecinds = (int)currentThread.currMethod._tracer.Elapsed.TotalMilliseconds;

            currentThread.currMethod = currentThread.currMethod.parent;

            if (currentThread.currMethod != null)
            {
                currentThread.parent = currentThread.currMethod.parent;
            }

        }
        
        public TraceResult GetTraceResult()
        {
            return new TraceResult(threads);
        }
    }
}