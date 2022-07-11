using System;
using System.Collections.Generic;
using UnityEngine;

public static class GpsConversions
{
    const double Er = 6378137;
    //const double a2 = 40680631590769;
    //const double b = 6356752.3142;
    //const double b2 = 40408299984087.05552164;
    const double e2 = 0.00669437999014;
    //const double b2_a2 = 0.99330561999573919348441651272333;
    const double deg2rad = Math.PI / 180d;
    public static Vector3 origin;


    public static double ref_lat;
    public static double ref_lon;
    public static double ref_alt;

    public static Vector3 InitialiseOrigin(double lat, double lon)
    {
        origin = Vector3.zero;
        origin = AccuratePositionFromLatLonH(lat, lon, 0.0d);


        ref_lat = lat;
        ref_lon = lon;

        return origin;
    }

    public static Vector3 InitialiseOrigin(double lat, double lon, double alt)
    {
        origin = Vector3.zero;
        origin = AccuratePositionFromLatLonH(lat, lon, alt);

        ref_lat = lat;
        ref_lon = lon;
        ref_alt = alt;

        return origin;
    }


    public static Vector3 AccuratePositionFromLatLonH(double lat, double lon, double alt)
    {
        //    lat = deg2rad * lat;
        //    lon = deg2rad * lon;

        double SinLat = Math.Sin(lat);
        double SinLon = Math.Sin(lon);
        //double CosLat = Math.Cos(lat);
        //double CosLon = Math.Cos(lon);


        double R_ea = 6378137.0;

        double e = 0.08181919;

    double Ne = R_ea / (Math.Sqrt(1.0 - (e * e * Math.Sin(deg2rad*lat) * Math.Sin(deg2rad*lat))));




        double ecef_x = (Ne + alt) * Math.Cos(deg2rad*lat) * Math.Cos(deg2rad*lon);

        double ecef_y = (Ne + alt) * Math.Cos(deg2rad*lat) * Math.Sin(deg2rad*lon);

        double ecef_z = ((Ne * (1.0 - e * e)) + alt) * Math.Sin(deg2rad*lat);




        double ecef_x_ref = (Ne + ref_alt) * Math.Cos(deg2rad*ref_lat) * Math.Cos(deg2rad*ref_lon);

        double ecef_y_ref = (Ne + ref_alt) * Math.Cos(deg2rad*ref_lat) * Math.Sin(deg2rad*ref_lon);

        double ecef_z_ref = ((Ne * (1.0 - e * e)) + ref_alt) * Math.Sin(deg2rad*ref_lat);




        double rel_x = ecef_x - ecef_x_ref;

        double rel_y = ecef_y - ecef_y_ref;

        double rel_z = ecef_z - ecef_z_ref;




        double ned_x = rel_x * (-1.0 * Math.Sin(deg2rad*ref_lat) * Math.Cos(deg2rad*ref_lon)) + rel_y * (-1.0 * Math.Sin(deg2rad*ref_lat) * Math.Sin(deg2rad*ref_lon)) + rel_z * (Math.Cos(deg2rad*ref_lat));

        double ned_y = rel_x * (-1.0 * Math.Sin(deg2rad*ref_lon)) + rel_y * (Math.Cos(deg2rad*ref_lon)) + rel_z * (0.0);

        double ned_z = rel_x * (-1.0 * Math.Cos(deg2rad*ref_lat) * Math.Cos(deg2rad*ref_lon)) + rel_y * (-1.0 * Math.Cos(deg2rad*ref_lat) * Math.Sin(deg2rad*ref_lon)) + rel_z * (-1.0 * Math.Sin(deg2rad*ref_lat));


        //double N = Er / Math.Sqrt(1 - (e2 * SinLat * SinLat));
        //double r = N + alt;

        //double x = r * SinLon;
        //double y = r;
        //double z = r * SinLat;

        return new Vector3((float)ned_y, (float)-ned_z, (float)ned_x);
    }

    public static Vector3 AccuratePositionFromLatLonH(double lat, double lon, double h, Vector3 origin)
    {
        lat = deg2rad * lat;
        lon = deg2rad * lon;

        double SinLat = Math.Sin(lat);
        double SinLon = Math.Sin(lon);
        //double CosLat = Math.Cos(lat);
        //double CosLon = Math.Cos(lon);

        double N = Er / Math.Sqrt(1 - (e2 * SinLat * SinLat));
        double r = N + h;

        double x = r * SinLon;
        double y = r;
        double z = r * SinLat;

        return new Vector3((float)x, (float)y, (float)z) - origin;
    }
}
