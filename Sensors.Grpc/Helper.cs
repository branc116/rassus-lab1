namespace Sensors.Grpc;
public static class Helper {
    public static int iptoint(this string ip) {
        var a = ip.Split(".").Select(int.Parse).ToArray();
        int ret = 0;
        for(int i = 0; i < 4;i++) {
            ret |= a[i] << (8*i);
        }
        return ret;
    }
    public static string inttoip(this int ip) {
        string s = "";
        for(int i = 0; i < 4;i++) {
            s += $"{ip >> (i * 8) & 0xFF}.";
        }
        return s;
    }

}