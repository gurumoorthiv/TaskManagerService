using NBench;
using TaskManagerBusinessLayer;
using TaskManagerDataAcessLayer;
using TaskManagerTestQualityTools;
using Unity;

namespace TaskControllerPerformanceTest
{

    public class TaskControllerPerformanceTest
        {
            private Counter perfCounter;
            private ITaskService service;
            private UnityContainer container;

            [PerfSetup]
            public void Setup(BenchmarkContext context)
            {

                container = new UnityContainer();
                container.RegisterType<ITaskDbContext, TaskDbContextFake>();
                container.RegisterType<ITaskService, TaskService>();
                service = container.Resolve<ITaskService>();

                //perfCounter = context.GetCounter("PeformanceCounter");
            }
            //[PerfBenchmark(NumberOfIterations =5, RunMode=RunMode.Throughput, RunTimeMilliseconds =500,TestMode =TestMode.Measurement)]
            //[CounterMeasurement("PeformanceCounter")]
            //[MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
            //[GcMeasurement(GcMetric.TotalCollections,GcGeneration.AllGc)]

            //public void BenchmarkMethod(BenchmarkContext context)
            //{
            //    //service.GetAllTasks();
            //    //perfCounter.Increment();
            //}

            [PerfBenchmark(NumberOfIterations = 1, RunMode = RunMode.Throughput, TestMode = TestMode.Test, SkipWarmups = true)]
            [ElapsedTimeAssertion(MaxTimeMilliseconds = 5000)]
            public void Benchmark_Performance_ElaspedTime()
            {
                service.GetAllTasks();
            }

            [PerfBenchmark(RunMode = RunMode.Iterations, TestMode = TestMode.Measurement)]
            [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
            public void Benchmark_Performance_GC()
            {
                service.GetAllTasks();
            }

            [PerfBenchmark(NumberOfIterations = 1, RunMode = RunMode.Throughput, RunTimeMilliseconds = 2500, TestMode = TestMode.Test)]
            [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
            //[MemoryAssertion(MemoryMetric.TotalBytesAllocated, MustBe.LessThanOrEqualTo, ByteConstants.SixtyFourKb)]
            public void Benchmark_Performance_Memory()
            {
                service.GetAllTasks();
            }
        }
}
