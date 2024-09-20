namespace CodeCheck
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<double> A = new List<double>();
            List<double> B = new List<double>();

            Console.WriteLine("Enter the size of list A (M):");
            int M = int.Parse(Console.ReadLine());

            Console.WriteLine($"Enter {M} elements for list A:");
            for (int i = 0; i < M; i++)
            {
                double element = double.Parse(Console.ReadLine());
                A.Add(element);
            }

            Console.WriteLine("List A contains the following elements:");
            Console.WriteLine(string.Join(" ", A));

            // BEGINING
            // MAIN TASK - FIND X
            double S = 0;
            double K = 0;
            for (int i = 0; i < M; i++)
                if (A[i] < 0)
                {
                    S += A[i];
                    K++;
                }
            if (M == 0 || K == 0)
            {
                // DO NOT COUNT THESE FUNCS
                Console.WriteLine("ERROR");
                return;
            }
            double avg = S / K;
            int x_ind = 0;
            double mx = -1;
            /*double mx = Math.Abs(A[x_ind] - avg);*/
            for (int i = 1; i < M; i++)
            {
                double cur = Math.Abs(A[i] - avg);
                if (cur > mx)
                {
                    mx = cur;
                    x_ind = i;
                }
            }
            // OUTPUT RESULT
            Console.WriteLine($"X = {A[x_ind]}");
            // TASK 1
            {
                int b_ind = 0;
                for (int i = 0; i < x_ind; i++)
                    if (A[i] > 0 && A[i] % 2 == 1)
                    {
                        B.Add(A[i]);
                        B[b_ind] = A[i];
                        b_ind++;
                    }
            }
            // OUTPUT RESULT
            Console.WriteLine("List B contains the following elements:");
            Console.WriteLine(string.Join(" ", B));
            // TASK 2
            {
                for (int i = 0; i < M; i++)
                {
                    if ((A[i] > 0 && i < x_ind) || (A[i] > 0 && A[i] % 2 == 1 && i > x_ind))
                    {
                        for (int j = i; j < M - 1; j++)
                            A[j] = A[j + 1];
                        M--;
                        i--;
                        x_ind--;
                    }
                }
            }
            // OUTPUT RESULT
            Console.WriteLine("List A contains the following elements:");
            Console.WriteLine(string.Join(" ", A));
            Console.WriteLine("But indeed true content of A is:");
            for (int i = 0; i < M; i++)
                Console.WriteLine(A[i]);

        }
    }
}
