using System.Collections.Generic;
using System.Net;
using System.Threading;
using ThreadSafeCollections;

namespace PortProberLib
{
    public class ThreadedPortProber
    {
        private SynchronizedObservableCollection<KeyValuePair<int, bool>> results;

        private ReadOnlySynchronizedObservableCollection<KeyValuePair<int, bool>> Results;
        private List<Thread> threads;

        public void RunProbes(IList<IPEndPoint> socketsToProbe, int[] associatedKeys)
        {
            results = new SynchronizedObservableCollection<KeyValuePair<int, bool>>();
            Results = new ReadOnlySynchronizedObservableCollection<KeyValuePair<int, bool>>(results);
            threads = new List<Thread>();

            for (int i = 0; i < socketsToProbe.Count; i++)
            {
                KeyValuePair<IPEndPoint, int> pair = new KeyValuePair<IPEndPoint, int>(socketsToProbe[i],
                                                                                       associatedKeys[i]);
                Thread thread = new Thread(Start);
                thread.Start(pair);
                threads.Add(thread);
            }
        }

        private void Start(object o)
        {
            KeyValuePair<IPEndPoint, int> pair = (KeyValuePair<IPEndPoint, int>) o;

            PortProber prober = new PortProber(pair.Key);
            bool success = prober.ProbeMachine();
            results.Add(new KeyValuePair<int, bool>(pair.Value, success));
        }
    }
}