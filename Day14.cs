﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    public static class Day14
    {
        public static long Part1(string[] lines)
        {

            string currentMask = "";
            var memory = new Int64[100000];

            foreach (var line in lines)
            {
                if (line.Substring(0, 3) == "mas")
                {
                    // set current mask
                    currentMask = line.Split('=')[1].TrimStart();
                }
                else if (line.Substring(0, 3) == "mem")
                {
                    // parse out values
                    int memLocation = int.Parse(line.Split('=')[0].TrimEnd().Replace("]", string.Empty).Substring(4));
                    int memValue = int.Parse(line.Split('=')[1].TrimStart());

                    string binaryValue = "000000000000000000000000000000000000" + Convert.ToString(memValue, 2);
                    int len = binaryValue.Length - 36;
                    binaryValue = binaryValue.Substring(len);

                    StringBuilder output = new StringBuilder();

                    for (int i = 0; i < 36; i++)
                    {
                        if (currentMask[i] != 'X')
                        {
                            output.Append(currentMask[i]);
                        }
                        else
                        {
                            output.Append(binaryValue[i]);
                        }
                    }

                    string complete = output.ToString();
                    memory[memLocation] = Convert.ToInt64(complete, 2);

                }
            }

            Int64 final = 0;

            foreach (var loc in memory)
            {
                final += loc;
            }

            return final;
        }

        public static long Part2(string[] lines)
        {
            var memory = new List<Tuple<long, long>>();

            string currentMask = "";

            foreach (var line in lines)
            {
                if (line.Substring(0, 3) == "mas")
                {
                    // set current mask
                    currentMask = line.Split('=')[1].TrimStart();
                }
                else if (line.Substring(0, 3) == "mem")
                {
                    // parse out values
                    int memLocation = int.Parse(line.Split('=')[0].TrimEnd().Replace("]", string.Empty).Substring(4));
                    int memValue = int.Parse(line.Split('=')[1].TrimStart());

                    // add leading 36 zeros to fill out to 36 bits
                    string binaryValue = "000000000000000000000000000000000000" + Convert.ToString(memLocation, 2);

                    // calc how many surplus zeros
                    int len = binaryValue.Length - 36;

                    // trim leading surplus
                    binaryValue = binaryValue.Substring(len);

                    StringBuilder output = new StringBuilder();

                    // iterate through bits applying mask
                    for (int i = 0; i < 36; i++)
                    {
                        if (currentMask[i] == 'X')
                        {
                            output.Append('X');
                        }
                        else if (currentMask[i] == '0')
                        {
                            output.Append(binaryValue[i]);
                        }
                        else if (currentMask[i] == '1')
                        {
                            output.Append('1');
                        }
                    }

                    string complete = output.ToString();
                    int max = complete.Count(x => x == 'X');
                    max = max * max;
                    var locations = new List<String>();
                    locations.Add(complete);

                    //we now have our floating memlocations

                    int count = 0;

                    // hacky while true loop. For loops fail due to modifying collection as we go
                    while (true)
                    {
                        for (int i = 0; i < 36; i++)
                        {
                            string loc = locations[count];
                            if (loc[i] == 'X')
                            {
                                // generate and add child floating addresses
                                locations.Add(loc.Substring(0, i) + "1" + loc.Substring(i + 1));
                                locations.Add(loc.Substring(0, i) + "0" + loc.Substring(i + 1));

                                // remove parent address from list and decrement 
                                // count to reflect this change
                                locations.Remove(loc);
                                count--;

                                // break out of the current foating address as we only
                                // wish to substitude one X per iteration
                                break;
                            }

                        }
                        count++;
                        if (count == max - 1) { break; }
                    }

                    foreach (var location in locations)
                    {
                        long workingLocation = Convert.ToInt64(location, 2);
                        //memory[Convert.ToInt64(location, 2)] = memValue;
                        var memoryVariable = memory.Where(x => x.Item1 == workingLocation);

                        if (memoryVariable.Count() != 0)
                        {
                            memory.Remove(memoryVariable.Single());
                        }

                        memory.Add(new Tuple<long, long>(workingLocation, memValue));
                    }
                }
            }

            Int64 final = 0;

            foreach (var loc in memory)
            {
                final += loc.Item2;
            }

            return final;
        }
    }
}