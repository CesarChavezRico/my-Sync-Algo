using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_1___Synchronization_Algorithm
{
    /// <summary>
    /// Yield points
    /// </summary>
    class Synchronizer
    {
        private readonly List<DataSource> _sources; 

        public Synchronizer(List<DataSource> sources)
        {
            _sources = sources;
        }

        public IEnumerable<Point> GetNextPoint()
        {
            while (true)
            {

                // ============== DO NOT DO BATCH PROCESSING ALGORITHMS LIKE THIS ==============
                //var output = new List<Point>();
                //foreach (var stream in streams)
                //{
                //    output.AddRange(stream.Generated.ToArray());
                //}
                //output = (from point in output orderby point.Time ascending select point).ToList();
                // ==============================================================================

                /*******************************************************************/
                // Your code goes here, yield return point by point to synchonize the data streams.

                var output = new List<Point>();
                var date_comparison = new List<Tuple<string, Point>>();
                var processed_streams = new List<string>();


                foreach (var stream in _sources)
                {
                    // get all the points on the top of each stream queue to compare dates
                    if (stream.Generated.Count > 0)
                    {
                        var p = stream.Generated.Peek();
                        date_comparison.Add(new Tuple<string, Point>(stream.Name, p));
                    }
                }

                date_comparison = (from point in date_comparison orderby point.Item2.Time ascending select point).ToList();
                foreach (var stream in _sources)
                {
                    // find the stream with the oldest point and yield return the top most point on the queue
                    if (date_comparison.Count > 0)
                    {
                        if (stream.Name == date_comparison[0].Item1)
                        {
                            var point_to_yeild = stream.Generated.Dequeue();
                            yield return point_to_yeild;
                        }
                    }
                    else
                    {
                        yield break;
                    }
                }






                /*******************************************************************/
            }
        }
    }
}
