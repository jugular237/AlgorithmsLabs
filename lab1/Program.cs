


string pathAFile = @"D:\Algorithms\algorithms\AFile.txt";
string pathB1File = @"D:\Algorithms\algorithms\B1File.txt";
string pathB2File = @"D:\Algorithms\algorithms\B2File.txt";
string pathC1File = @"D:\Algorithms\algorithms\C1File.txt";
string pathC2File = @"D:\Algorithms\algorithms\C2File.txt";

int AFilelngth = 2 * 1024 * 1024;

void FillFile()                                    // заповнюємо рандомними числами початковий файл "AFile"
{
    File.WriteAllText(pathAFile, string.Empty);
    File.WriteAllText(pathB1File, string.Empty);   //очищаємо файли
    File.WriteAllText(pathB2File, string.Empty);
    File.WriteAllText(pathC1File, string.Empty);
    File.WriteAllText(pathC2File, string.Empty);
    Random rnd = new Random();
    int randNum;
    using (FileStream fs = new FileStream(pathAFile, FileMode.Append, FileAccess.Write))
    using (StreamWriter sw = new StreamWriter(fs))
    {
        while (fs.Length < AFilelngth)
        {
            randNum = rnd.Next(0, Int32.MaxValue);
            sw.WriteLine($"{randNum}");
        }
    }
}

void SplitToOtherFiles()        // перепис з файлу AFile у допоміжні файли B1File, B2File
{
    using (FileStream fs = new FileStream(pathAFile, FileMode.Open, FileAccess.Read))
    using (StreamReader sr = new StreamReader(fs))
    {
        string line;
        string series = "";
        bool writeToB1 = true;
        int prevNum = 0;
        while((line = sr.ReadLine()) != null)       // розбиває всі числа на серії і записує в файли
        {
            if (int.Parse(line) < prevNum)
            {
                if(writeToB1)
                    WriteToFile(pathB1File, series); // cкладаємо серії
                else
                    WriteToFile(pathB2File, series);
                writeToB1 = !writeToB1;
                series = "";
            }
            
            series += line + " ";
            prevNum = int.Parse(line);
        }
    }
}

void Algorithm(string pathToWrite1, string pathToWrite2, string pathToRead1, string pathToRead2)
{
    File.WriteAllText(pathToWrite1, string.Empty);                      // очищуємо файли 
    File.WriteAllText(pathToWrite2, string.Empty);
    int length = File.ReadLines(pathToRead1).Count();
    int length1 = File.ReadLines(pathToRead2).Count();
    string line;
    int[][] file1Series = new int[length][];
    int[][] file2Series = new int[length1][];
    using (FileStream fs = new FileStream(pathToRead1, FileMode.Open, FileAccess.Read))
    using (StreamReader sr = new StreamReader(fs)) 
    {
        int i = 0;
        while ((line = sr.ReadLine()) != null)
        {
            file1Series[i] = line.TrimEnd().Split().Select(s => int.Parse(s)).ToArray();  // представляємо кожен рядок як массив
            i++;
        }
    }
    using (FileStream fs = new FileStream(pathToRead2, FileMode.Open, FileAccess.Read))
    using (StreamReader sr = new StreamReader(fs))
    {
        int i = 0;
        while ((line = sr.ReadLine()) != null)
        {
            file2Series[i] = line.TrimEnd().Split().Select(s => int.Parse(s)).ToArray();
            i++;
        }
    }
    int k = 0;
    int[] helpArr;
    if (length > length1)
    {
        foreach (var serie in file1Series)
        {
            if (k < length1)
            {
                helpArr = serie.Concat(file2Series[k]).ToArray();           // сoртуємо та зливаємо серії
                Array.Sort(helpArr);
            }
            else
                helpArr = serie;
            if (k % 2 == 0)
                WriteToFile(pathToWrite1, string.Join(" ", helpArr));
            else
                WriteToFile(pathToWrite2, string.Join(" ", helpArr));
            k++;
        }
    }
    else
    {
        foreach (var serie in file2Series)
        {
            if (k < length)
            {
                helpArr = serie.Concat(file1Series[k]).ToArray();
                Array.Sort(helpArr);
            }
            else
                helpArr = serie;
            if (k % 2 == 0)
                WriteToFile(pathToWrite1, string.Join(" ", helpArr));
            else
                WriteToFile(pathToWrite2, string.Join(" ", helpArr));
            k++;
        }
    }
}

void MergeResults(string pathResult1, string pathResult2)  // Зливаємо два фінальні допоміжні відсортовані файли в початкоий "AFile.txt"
{
    File.WriteAllText(pathAFile, string.Empty);
    string line;
    int[] file1Series;
    int[] file2Series;
    using (FileStream fs = new FileStream(pathResult1, FileMode.Open, FileAccess.Read))
    using (StreamReader sr = new StreamReader(fs))
    {
        file1Series = sr.ReadLine().TrimEnd().Split().Select(s => int.Parse(s)).ToArray();  // розділяємо всі числа рядка на массив чисел
    }
    using (FileStream fs = new FileStream(pathResult2, FileMode.Open, FileAccess.Read))
    using (StreamReader sr = new StreamReader(fs))
    {
        int i = 0;
        file2Series = sr.ReadLine().TrimEnd().Split().Select(s => int.Parse(s)).ToArray();
    }
    int k = 0;
    int[] helpArr;
    helpArr = file1Series.Concat(file2Series).ToArray();     // зєднуємо та сортуємо кінцевий массив
    Array.Sort(helpArr);
    WriteToFile(pathAFile, string.Join(" ", helpArr));       // записуємо його у файл
    
}

void SelectAndMergeSeries()  // чергує файли для зчитування та запису
{
    int i = 0;
    while (true)
    {
        if (File.ReadLines(pathB1File).Count() == 1 && File.ReadLines(pathB2File).Count() == 1) 
        {
            MergeResults(pathB1File, pathB2File);
            return;
        }
        if (File.ReadLines(pathC1File).Count() == 1 && File.ReadLines(pathC2File).Count() == 1)
        {
            MergeResults(pathC1File, pathC2File);
            return;
        }
        if (i % 2 == 0)
            Algorithm(pathC1File, pathC2File, pathB1File, pathB2File);
        else
            Algorithm(pathB1File, pathB2File, pathC1File, pathC2File);
        i++;
    }
}

void WriteToFile(string path, string number)        // заповнює допопміжний файл серіями
{
    using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write)) 
    using (StreamWriter sw = new StreamWriter(fs))
    {
        sw.WriteLine(number);
    }
}



//Main();

void Main()
{
    FillFile();
    SplitToOtherFiles();
    SelectAndMergeSeries();
}