file = open("log.txt", "r")

content = file.readlines()

sum = 0
cnt = 0

for item in content:
    if item[0] == 'W':
        cnt += 1
        sum += float(item[5: len(item)])
        
print("Average WPM: ", sum / cnt)