#include<iostream>
#include<cstdlib>
#include<ctime>

using namespace std;

void generate()
{
	int sumRow=0;
	int limLw[5] = {1,4,10,13,17};
	int limUp[5] = {9,17,23,27,29};
	int sumCol[4] = {0,0,0,0};
	
	int lw, up;
	
	for(int i=0; i<5;i++)
	{
		sumRow=0;
		
		cout << "\t| ";
		for(int j=0; j<4;j++)
		{
			lw = max(max(1,limLw[j]-sumRow),limLw[i]-sumCol[j]);
			up = min(min(9,limUp[j]-sumRow),limUp[i]-sumCol[j]);
			
			int rng = rand()%(up-lw+1) + lw;
			//int rng = lw; //force lower|upper limit
			sumRow+=rng;
			sumCol[j]+=rng;
			
			cout << rng << "\t| ";
			
			//==========================
		}
		cout << "== "<< sumRow << "\t|\n";
	}
	
	cout << "\t| = " << sumCol[0] << "\t| = " << sumCol[1] << "\t| = " << sumCol[2] << "\t| = " << sumCol[3] <<"\t| ===== |\n";
	
	return;
}

int main()
{
	cout <<"\n\n==============================================================\n\n";
	
	char porno = 'm';
	srand(time(0));
	while(true)
	{
		generate();
		cin.ignore();
	}
	
	cout <<"\n\n==============================================================\n\n";
	
	//cout << 0%0;
	
	
	
	return 0;
}








