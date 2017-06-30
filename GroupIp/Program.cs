using System;
using System.Linq;
using System.Collections.Generic;

namespace GroupIp
{
    public enum ResourceType
    {
        IP = 0,
        IPPattern = 1,
        Group = 2,
        Country = 3,
    }

    public class Resource
    {
        public ResourceType Type { get; set; }
        public string Data { get; set; }
    }

    public class Group
    {
        public string GroupId { get; set; }
        public IEnumerable<Resource> Resources { get; set; }
    }

    public class Rule
    {
        public IEnumerable<Resource> Resources { get; set; }
        public bool Allow { get; set; }
    }

    class Program
    {

        static Group groupA = new Group
        {
            GroupId = "A",
            Resources = new Resource[] {
                    new Resource {
                        Type = ResourceType.Group,
                        Data = "B"
                    },
                    new Resource {
                        Type = ResourceType.IP,
                        Data = "4.5.6.7"
                    }
                }
        };

        static Group groupB = new Group
        {
            GroupId = "B",
            Resources = new Resource[] {
                    new Resource {
                        Type = ResourceType.Group,
                        Data = "C"
                    },
                    new Resource {
                        Type = ResourceType.IP,
                        Data = "7.8.9.10"
                    },
                    new Resource {
                        Type = ResourceType.Group,
                        Data = "A"
                    }
                }
        };

        static Group groupC = new Group
        {
            GroupId = "C",
            Resources = new Resource[] {
                    new Resource {
                        Type = ResourceType.Group,
                        Data = "A"
                    }
                }
        };

        static Group[] groups = new Group[] {
            groupA, groupB, groupC
        };

        static void Main(string[] args)
        {
            var rule =
                new Rule
                {
                    Allow = true,
                    Resources = new Resource[] {
                        new Resource {
                            Type = ResourceType.Group,
                            Data = "A"
                        },
                        new Resource {
                            Type = ResourceType.Group,
                            Data = "B"
                        },
                        new Resource {
                            Type = ResourceType.IP,
                            Data = "1.2.3.4"
                        },
                    }
                };

            var data = ResolveData(rule.Resources);

            foreach (var d in data)
            {
                Console.WriteLine(d);
            }

            Console.ReadKey();

            var values = new[] {
                new { x = "hello", y = Test.Hello },
                new { x = "world", y = Test.World },
                new { x = "hello", y = Test.Hello },
                new { x = "world", y = Test.Hello },
            };

            foreach (var v in values.Distinct())
            {
                Console.WriteLine("{0} : {1}", v.x, v.y);
            }

            Console.ReadKey();
        }

        public enum Test
        {
            Hello,
            World
        }

        private static IEnumerable<string> ResolveDataDebug(IEnumerable<Resource> resources, IEnumerable<Resource> visited)
        {
            return resources
                .SelectMany(i =>
                {
                    Console.WriteLine("Visiting " + i.Data);
                    Console.WriteLine("Visited [{0}]", string.Join("|", visited.Select(v => v.Type.ToString() + " - " + v.Data)));

                    if (visited.Contains(i))
                    {
                        Console.WriteLine("Already visited.");
                        return new string[] { };
                    }

                    if (i.Type == ResourceType.Group)
                    {
                        return ResolveDataDebug(groups.First(g => g.GroupId == i.Data).Resources, visited.Append(i));
                    }

                    return new string[] { i.Data };
                });
        }

        private static IEnumerable<string> ResolveData(IEnumerable<Resource> resources)
        {
            IEnumerable<string> resolve(IEnumerable<Resource> r, IEnumerable<Resource> visited)
            {
                return r.SelectMany(i =>
                    {
                        if (visited.Contains(i))
                            return Enumerable.Empty<string>();

                        if (i.Type == ResourceType.Group)
                            return resolve(
                                groups.FirstOrDefault(g => g.GroupId == i.Data)
                                    ?.Resources
                                    ?? Enumerable.Empty<Resource>(),
                                visited.Append(i));

                        return Enumerable.Empty<string>()
                            .Append(i.Data);
                    });
            }

            return resolve(resources, Enumerable.Empty<Resource>())
                .Distinct();
        }
    }
}