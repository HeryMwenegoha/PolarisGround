/* Hery A Mwenegoha (C) 2017
 * Compute the Jacobian 
 * 
 */ 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
namespace UGCS3.INS_Calibration
{
    class AccelCalibration
    {
        float xraw, yraw,zraw;
#if !ACCEL_1
        float b1 = 0F, b2 = 0F, b3 = 0F, b4 = 8192F, b5 = 8192F, b6 = 8192F;
#else
        float b1 = 0F, b2 = 0F, b3 = 0F, b4 = 500F, b5 = 500F, b6 = 500F;
#endif

        float[,] JtJr =  new float[6,6]
        {
          {0,0,0,0,0,0},
          {0,0,0,0,0,0},
          {0,0,0,0,0,0},
          {0,0,0,0,0,0},
          {0,0,0,0,0,0},
          {0,0,0,0,0,0},
        };

        float[] Jr = {0,0,0,0,0,0};       

        public AccelCalibration()
        {

        }

        // Update the Jacobian Matrices
        private void update_matrices()
        {	
          float drdb1 = (float)(2.0*(xraw - b1)/Math.Pow(b4,2));
          float drdb2 = (float)(2.0*(yraw - b2)/Math.Pow(b5,2));
          float drdb3 = (float)(2.0*(zraw - b3)/Math.Pow(b6,2));
		
          float drdb4 = (float)(2.0*((xraw - b1)*(xraw - b1))/Math.Pow(b4,3));
          float drdb5 = (float)(2.0*((yraw - b2)*(yraw - b2))/Math.Pow(b5,3));
          float drdb6 = (float)(2.0*((zraw - b3)*(zraw - b3))/Math.Pow(b6,3));
    
          float ax    = (xraw - b1)/b4;
          float ay    = (yraw - b2)/b5;
          float az    = (zraw - b3)/b6;
		
          float ax_sq = ax*ax;
          float ay_sq = ay*ay;
          float az_sq = az*az;
		
          float Residual = 1 -(ax_sq + ay_sq + az_sq); // scalar quantity
  
          float[] Jacobian = {drdb1,drdb2,drdb3,drdb4,drdb5,drdb6}; 
						
          for(int r = 0; r<6; r++)
          {
            for(int c = 0; c<6; c++)
            {
               JtJr[r,c] += Jacobian[r]*Jacobian[c];   					
            }					
            Jr[r] += Jacobian[r] * Residual;
          }     
        }


        // Resets the Jacobian Matrices before computation
        private void reset_matrices()
        {
           for(int r = 0; r<6; r++)
           {
              for(int c = 0; c<6; c++)
              {
                JtJr[r,c] = 0.0f;   					
              }					
              Jr[r]        = 0.0f;
           }
        }


        // Performs a row switch to implement partial pivoting
        private void row_switch(ref float[,] Hess,  int  pivrow,  int switchrow)
        {
          for(int col = 0; col<7; col++)
          {
            float pivmat          =  Hess[pivrow, col];
            float switchmat 	  =  Hess[switchrow, col];
            Hess[pivrow ,col]     =  switchmat;
            Hess[switchrow, col]  =  pivmat;			
          }
        }


        // Performs Gaussian elimination with back substitution to solve a system of 6 linear equations
        private bool gauselimination_pivoting(float []Delta)
        {        
            float[,] Gauss_Matrix = new float[6,7] 
            {
                {0,  0,   0,   0,   0,   0 , 0},
                {0,  0,   0,   0,   0,   0 , 0},
                {0,  0,   0,   0,   0,   0 , 0},
                {0,  0,   0,   0,   0,   0 , 0},
                {0,  0,   0,   0,   0,   0 , 0},
                {0,  0,   0,   0,   0,   0 , 0},
            };
  
            // populate gauss matrix
            for(int r = 0; r<6; r++)
            {
                for(int c = 0; c<6; c++)
                {
                    Gauss_Matrix[r,c] = JtJr[r,c];
                }
                Gauss_Matrix[r,6]   = Jr[r];
            }
  	
            //int diag_index     = 0;
            const int rows     = 6;
            const int columns  = 7;
            int pivrow         = 0;  // the row to be moved from its position to switchrow.
            int pivcol         = 0;
            int switchrow      = 0;  // the row that will have to move to pivrow if it has the max absolute value
            float temp         = 0;  // stores maximum value
	
            for(int diag = 0; diag < (rows-1); diag++)
            {
                // Set the position to start checking for the max value to perform pivoting (row switching). 
                pivrow     = diag;	
                pivcol     = diag;	
                switchrow  = diag;
							
                // iterate down the rows starting from the pivrow and pivcolumn			
                for(int pivindex = pivrow; pivindex < rows; pivindex++)
                {
	                if(Math.Abs(Gauss_Matrix[pivindex,pivcol]) >= temp)
	                {
	                    temp = Math.Abs(Gauss_Matrix[pivindex,pivcol]); // the max value identified 
	                    switchrow = pivindex;					         // the row that needs to be switched to the current pivot row.
	                }
                }
			
                // check for singularity in the pivot row (which in real sense should be restated as the row that has the largest diagonal element)
                // but for now this is the switchrow
                if(Gauss_Matrix[switchrow,diag] == 0)
                {
	                return false;
                }
			
                // Perform Pivoting, row switching before forward elimination
                if(switchrow != pivrow)
                {
                    row_switch(ref Gauss_Matrix, pivrow, switchrow);
                }
			
                // Get the max value of the diagonal element for the forward elimination
                // Forward Elimination
                // Happens from current diag-row +1 
                // for each row calculate the multiplying factor for the diagonal row
						
                for(int i = (diag+1); i<rows; i++)
                {
                    int pivotIndex  = diag;
                    int nextIndex    = i;
				
                    float max_pivot              = Gauss_Matrix[pivotIndex,pivotIndex];
                    float FirstVector_Multfactor = Gauss_Matrix[nextIndex ,pivotIndex]/max_pivot;
				
                    // iterate through the column and perform subtraction/elimination
                    for(int x = 0; x < columns; x++)
                    {				
	                    float FirstVector_Value     = Gauss_Matrix[pivotIndex,x] * FirstVector_Multfactor;
	                    float SecondVector_Value    = Gauss_Matrix[nextIndex, x];
	                    float DiffVector_Value      = FirstVector_Value - SecondVector_Value;
	                    Gauss_Matrix[nextIndex, x]  = DiffVector_Value; // substitute value back into the gaussian matrix
                    }
                 }
             }
        
            // Answers to the 6 unknowns in the equation
            for(int y = 5; y>= 0; y--)
            {
                switch(y)
                {
                    case 5:
                    Delta[y] = Gauss_Matrix[y,6] / Gauss_Matrix[y,y];
                    break;
          
                    case 4:
                    Delta[y] = (Gauss_Matrix[y,6] - Gauss_Matrix[y,(y+1)] * Delta[y+1]) / Gauss_Matrix[y,y];
                    break;
          
                    case 3:
                    Delta[y] = (Gauss_Matrix[y,6] - (Gauss_Matrix[y,(y+1)] * Delta[y+1]) - (Gauss_Matrix[y,(y+2)] * Delta[y+2])) / Gauss_Matrix[y,y];
                    break;
          
                    case 2:
                    Delta[y] = (Gauss_Matrix[y,6] - (Gauss_Matrix[y,(y+1)] * Delta[y+1]) - (Gauss_Matrix[y,(y+2)] * Delta[y+2]) - (Gauss_Matrix[y,(y+3)] * Delta[y+3])) / Gauss_Matrix[y,y];
                    break;
          
                    case 1:
                    Delta[y] = (Gauss_Matrix[y,6] - (Gauss_Matrix[y,(y+1)] * Delta[y+1]) - (Gauss_Matrix[y,(y+2)] * Delta[y+2]) - (Gauss_Matrix[y,(y+3)] * Delta[y+3])  - (Gauss_Matrix[y,(y+4)] * Delta[y+4])) / Gauss_Matrix[y,y];
                    break;
          
                    case 0:
                    Delta[y] = (Gauss_Matrix[y,6] - (Gauss_Matrix[y,(y+1)] * Delta[y+1]) - (Gauss_Matrix[y,(y+2)] * Delta[y+2]) - (Gauss_Matrix[y,(y+3)] * Delta[y+3])  - (Gauss_Matrix[y,(y+4)] * Delta[y+4]) - (Gauss_Matrix[y,(y+5)] * Delta[y+5])) / Gauss_Matrix[y,y];
                    break;
                }
            }
	        return true;
        }


        // function that calibrates the accelerometer
        private void calibrate_accelerometer(Int16[] xvector, Int16[] yvector, Int16[] zvector)
        { 
            // Solve for delta Matrix::
            // Convergence Criteria -> violate loop if one of the criterion is met
            int num_of_iterations = 20;
            float eps             = 0.000000001F;
            float change          = 100;
 
            while(num_of_iterations-- > 0 && change > eps)
            {  
                // reset all matrices  
                reset_matrices();
    
                // populate matrices using beta and xraw
                for(int i = 0; i<120; i++)
                {
                    // xraw values
                    xraw = (float)xvector[i];
                    yraw = (float)yvector[i];
                    zraw = (float)zvector[i];
      
                    // calculate calibration matrices   
                    update_matrices();
                }
     
                // solve for delta
                float[] Delta = {0,0,0,0,0,0};
                if(gauselimination_pivoting(Delta))
                {
                    // size of change
                    change = Delta[0] * Delta[0] + Delta[1] * Delta[1] + Delta[2] * Delta[2] + Delta[3] * Delta[3]/(b4*b4) + Delta[4] * Delta[4]/(b5*b5) + Delta[5]*Delta[5]/(b6*b6);
       
                    // adjust beta
                    b1 -= Delta[0];
                    b2 -= Delta[1];
                    b3 -= Delta[2];
                    b4 -= Delta[3];
                    b5 -= Delta[4];
                    b6 -= Delta[5];
                 }
                 else
                 {
                     Console.WriteLine("Iteration " + num_of_iterations + " Failed");
                 }
            }


            if(change <= eps || num_of_iterations <= 0)
            {         
                Console.WriteLine("OFFSETS: ");
                Console.WriteLine(b1.ToString() + "     " + b2.ToString() + "       " + b3.ToString());
                Console.WriteLine("GRAVITY: ");
                Console.WriteLine(b4.ToString() + "     " + b5.ToString() + "       " + b6.ToString());

                System.Windows.Forms.MessageBox.Show(
                    "Accelerometer Calibration Succesful"
                    + '\n'
                    + "OFFSETS: "
                    + '\n'
                    + "B1 " + b1
                    + '\n'
                    + "B2 " + b2
                    + '\n'
                    + "B3 " + b3
                    + '\n'
                    + "GRAVITY: "
                    + '\n'
                    + "B4 " + b4
                    + '\n'
                    + "B5 " + b5
                    + '\n'
                    + "B6 " + b6, "Succes",System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk
                    );
            }
            else
            {
                Console.WriteLine("Calibration  Failed");

                System.Windows.Forms.MessageBox.Show("Calibration Failed", "Alert", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }

        }

        // This is the Top most function called by the user [Public]
        // Prompt user to place the vehicle on the axis we want to collect.
        // 20 samples are collected over each axis leading to 120 samples per axis for a 6 point calibration  
        // 100ms delay is meant to address the deliberate delay in data transmission [10Hz]
        // The samples collected as axis vectors are passed over to the function f(Calibrate_Accelerometer) which performs the actual calibration
        public void Sample_Processing()
        {
            int x                 = 0;
            byte samples_per_axis = 20;
            byte number_of_axes   = 6;           // x y z -x -y -z
            int sample_count      = number_of_axes * samples_per_axis; 
         
            Int16[] xvector = new Int16[120]; 
            Int16[] yvector = new Int16[120];
            Int16[] zvector = new Int16[120];

            // NED - TRUE
            SpeechSynthesizer _speech = new SpeechSynthesizer();
           
            _speech.SpeakAsync("Place vehicle level and press any key");   // 
            if (System.Windows.Forms.DialogResult.OK == System.Windows.Forms.MessageBox.Show("Place vehicle level and press any key","",System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk))
            {
                while(x < samples_per_axis*1)
                {       
                    xvector[x] = Variables.accelX; 
                    yvector[x] = Variables.accelY;
                    zvector[x] = Variables.accelZ;
                    Console.WriteLine(xvector[x].ToString() + "     " + yvector[x].ToString() + "       " + zvector[x].ToString() + "      " + x);
                    x++;
                    System.Threading.Thread.Sleep(100);
                }          
            }

            _speech.SpeakAsync("Place vehicle right and press any key");   // 
            if (System.Windows.Forms.DialogResult.OK == System.Windows.Forms.MessageBox.Show("Place vehicle right and press any key", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk))
            {
                while(x < samples_per_axis*2)
                {
                    xvector[x] = Variables.accelX;
                    yvector[x] = Variables.accelY;
                    zvector[x] = Variables.accelZ;
                    Console.WriteLine(xvector[x].ToString() + "     " + yvector[x].ToString() + "       " + zvector[x].ToString() + "      " + x);
                    x++;
                    System.Threading.Thread.Sleep(100);
                }          
            }


            _speech.SpeakAsync("Place vehicle left and press any key");   
            if (System.Windows.Forms.DialogResult.OK == System.Windows.Forms.MessageBox.Show("Place vehicle left and press any key", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk))
            {
                while(x < samples_per_axis*3)
                {
                    xvector[x] = Variables.accelX;
                    yvector[x] = Variables.accelY;
                    zvector[x] = Variables.accelZ;
                    Console.WriteLine(xvector[x].ToString() + "     " + yvector[x].ToString() + "       " + zvector[x].ToString() + "      " + x);
                    x++;
                    System.Threading.Thread.Sleep(100);
                }          
            }

            _speech.SpeakAsync("Place vehicle Nose Down and press any key");
            if (System.Windows.Forms.DialogResult.OK == System.Windows.Forms.MessageBox.Show("Place vehicle Nose Down and press any key", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk))
            {
                while(x < samples_per_axis*4)
                {
                    xvector[x] = Variables.accelX;
                    yvector[x] = Variables.accelY;
                    zvector[x] = Variables.accelZ;
                    Console.WriteLine(xvector[x].ToString() + "     " + yvector[x].ToString() + "       " + zvector[x].ToString() + "      " + x);
                    x++;
                    System.Threading.Thread.Sleep(100);
                }          
            }

            _speech.SpeakAsync("Place vehicle Nose Up and press any key");
            if (System.Windows.Forms.DialogResult.OK == System.Windows.Forms.MessageBox.Show("Place vehicle Nose Up and press any key", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk))
            {
                while(x < samples_per_axis*5)
                {
                    xvector[x] = Variables.accelX;
                    yvector[x] = Variables.accelY;
                    zvector[x] = Variables.accelZ;
                    Console.WriteLine(xvector[x].ToString() + "     " + yvector[x].ToString() + "       " + zvector[x].ToString() + "      " + x);
                    x++;
                    System.Threading.Thread.Sleep(100);
                }          
            }

            _speech.SpeakAsync("Place vehicle Upside Down and press any key");
            if (System.Windows.Forms.DialogResult.OK == System.Windows.Forms.MessageBox.Show("Place vehicle Upside Down and press any key", "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk))
            {
                while(x < samples_per_axis*6)
                {
                    xvector[x] = Variables.accelX;
                    yvector[x] = Variables.accelY;
                    zvector[x] = Variables.accelZ;
                    Console.WriteLine(xvector[x].ToString() + "     " + yvector[x].ToString() + "       " + zvector[x].ToString() + "      " + x);
                    x++;
                    System.Threading.Thread.Sleep(100);
                }          
            }  
         
            // calibrate accelerometer
            calibrate_accelerometer(xvector, yvector, zvector);

            _speech.Dispose();        
        }
    }
}
