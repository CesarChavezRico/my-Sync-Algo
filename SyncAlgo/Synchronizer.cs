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

                // Priority queue to store the top point of each stream
                var pq = new PriorityQueue<(DataSource stream, Point point), DateTime>();

                // Initialize the priority queue with the top points of each stream
                foreach (var stream in _sources)
                {
                    if (stream.Generated.Count > 0)
                    {
                        var p = stream.Generated.Peek();
                        pq.Enqueue((stream, p), p.Time);  // p.Time is used as the priority
                    }
                }

                // While the priority queue is not empty, process the DataSources
                while (pq.Count > 0)
                {
                    // Dequeue the DataSource with the earliest point
                    var (stream, point) = pq.Dequeue();

                    // Yield the point
                    yield return stream.Generated.Dequeue();  // Return and remove the point from the queue
                    
                    // If the stream still has points, enqueue the next one
                    if (stream.Generated.Count > 0)
                    {
                        var next_point = stream.Generated.Peek();
                        pq.Enqueue((stream, next_point), next_point.Time);
                    }
                }

                Console.WriteLine("All streams processed, exiting.");
                yield break;

                /*******************************************************************/
            }
        }
    }
}
