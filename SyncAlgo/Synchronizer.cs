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

                var date_comparison = new List<Tuple<string, Point>>();
                // Filter out streams with empty Generated queues
                var valid_streams = _sources.Where(stream => stream.Generated.Count > 0).ToList();

                if (valid_streams.Count <= 0)
                {
                   yield break; 
                }

                foreach (var stream in valid_streams)
                {
                    // get all the points on the top of each valid stream queue to compare dates
                    var p = stream.Generated.Peek();
                    date_comparison.Add(new Tuple<string, Point>(stream.Name, p));
                }

                date_comparison = (from point in date_comparison orderby point.Item2.Time ascending select point).ToList();
                foreach (var stream in valid_streams)
                {
                    // find the stream with the oldest point and yield return the top most point on the queue
                    if (stream.Name == date_comparison[0].Item1)
                    {
                        var point_to_yeild = stream.Generated.Dequeue();
                        yield return point_to_yeild;
                    }
                }






                /*******************************************************************/
            }
        }
    }
}
