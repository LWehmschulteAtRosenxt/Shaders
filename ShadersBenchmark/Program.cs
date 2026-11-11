using BenchmarkDotNet.Running;
using ShadersBenchmark;

BenchmarkRunner.Run<ShaderBenchmark>(new BenchmarkDotNet.Configs.DebugInProcessConfig());