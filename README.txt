Hi, this is my ID validation program.

To start please run the below .exe shortcut.
IDValidation\IDValidation.exe - Shortcut.lnk

Thanks
Max.

How it works:
For a code to be valid

1.
Multiply each of the first nine digits by a weighting factor as follows:
Digit Position - Weighting Factor
1 - 11
2 - 10
3 - 9
4 - 8
5 - 7
6 - 6
7 - 5
8 - 4
9 - 3

2.
Add the 9 results together.

3.
Divide the total by 12 and establish the remainder.

4.
Subtract the remainder from 12.


If the result is 10 or 12 then the ID must be invalid.
If the result is 11 then the expected value of the check digit is 0.
Otherwise, the result is the expected value of the check digit.

Example valid codes:
7859089345
6846869352
6841458511 
5970570118 
5081617317 
4257451659 
2403033374 
1837780620 
0525925273 
0011942946 


--Unfortunately I do not have an example JSON file with IDs to be validated however this functionality does work.
