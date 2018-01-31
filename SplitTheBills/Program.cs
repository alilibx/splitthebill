using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplitTheBills
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Parameters
                string filename = "billsplit", filepath = "";
                int peopleintripcount = 0, repiptsstartindex = 0, grouprecipts = 0;
                List<float> participantcontrib = new List<float>();
                List<string> participantshare = new List<string>();
                float contribution = 0;
                string[] filecontent;


                // Read the text file name which has the parameters from user input          
                filepath = AppDomain.CurrentDomain.BaseDirectory + filename + ".txt";

                //infomative messages
                Console.WriteLine("Split The Bill");
                Console.WriteLine("------------------");
                Console.WriteLine("Input File Path:  " + filepath);

                //read file contents               
                //remove the empty lines from file
                filecontent = File.ReadAllLines(filepath).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                Console.WriteLine("File read succesful");

                //calculate recipts value for each participant
                while (Int16.Parse(filecontent[repiptsstartindex]) != 0)
                {
                    //get trip participants number from first row
                    peopleintripcount = Int16.Parse(filecontent[repiptsstartindex == 1 ? 0 : repiptsstartindex]);
                    //increase row index 
                    repiptsstartindex++;
                    //participants recipts loop to get each recipt group
                    for (int recgroup = 1; recgroup <= peopleintripcount; recgroup++)
                    {
                        // get the group recipt count from the first row in the recipt group
                        grouprecipts = short.Parse(filecontent[repiptsstartindex]);
                        //goto the next row to start reading recipts values
                        repiptsstartindex++;
                        //loop to read recipt values for the selected group 
                        for (int recipts = 1; recipts <= grouprecipts; recipts++)
                        {
                            //summation of the recipt value per group
                            contribution += float.Parse(filecontent[repiptsstartindex]);
                            //goto the next row
                            repiptsstartindex++;
                        }

                        //store the summation of the revipts for each participant for further calculation
                        participantcontrib.Add(contribution);
                        //reset the contribution variable for the next trip                   
                        contribution = 0;
                    }
                    //loop through recipt groups to calculate participant share by subtracting the average
                    // amoount of the recipts from the partipant's total contribution to specify his share value
                    for (int recgroup = 0; recgroup < peopleintripcount; recgroup++)
                    {
                        //store the calculations in an array for sending to the output file and move to the next trip
                        participantshare.Add((participantcontrib.Average() - participantcontrib[recgroup]).ToString("#,##0.00;(#,##0.00)"));                        
                    }

                    //clear the trip participants contribution array to be ready for the next trip parameteres
                    participantcontrib.Clear();
                    participantshare.Add("");
                }


                //output results to file
                File.Delete(filepath + ".out");
                File.WriteAllLines(filepath + ".out", participantshare);
                Console.WriteLine("Output file created successfuly");
                Console.WriteLine("Output File Path:  " + filepath + ".out");

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            //to stop the screen from closing :) Work arround :D
            Console.Read();
        }         
    }
}
