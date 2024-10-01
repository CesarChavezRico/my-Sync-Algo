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

                // Filter out streams with empty queues
                var valid_streams = _sources.Where(stream => stream.Generated.Count > 0).ToList();
                if (valid_streams.Count <= 0)
                {
                    yield break;
                }

                // Collect the top point from each valid stream
                foreach (var stream in valid_streams)
                {
                    var p = stream.Generated.Peek();
                    date_comparison.Add(new Tuple<string, Point>(stream.Name, p));
                }

                // Sort points by their time in ascending order
                date_comparison = date_comparison.OrderBy(point => point.Item2.Time).ToList();

                while (date_comparison.Count > 0)
                {
                    // Find the stream with the oldest point and dequeue it
                    var oldest_stream_name = date_comparison[0].Item1;
                    var stream = valid_streams.First(s => s.Name == oldest_stream_name);
                    var point_to_yield = stream.Generated.Dequeue();
                    yield return point_to_yield;

                    // If the stream is now empty, remove it
                    if (stream.Generated.Count == 0)
                    {
                        valid_streams.Remove(stream);
                        date_comparison.RemoveAt(0); // Remove the processed stream from comparison
                    }
                    else
                    {
                        // Update the top point for this stream in the comparison list
                        var new_point = stream.Generated.Peek();
                        date_comparison[0] = new Tuple<string, Point>(stream.Name, new_point);
                        
                        // Re-sort to maintain the correct order
                        date_comparison = date_comparison.OrderBy(point => point.Item2.Time).ToList();
                    }
}






                /*******************************************************************/
            }
        }
    }
}
