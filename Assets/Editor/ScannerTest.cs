﻿using BarcodeScanner;
using BarcodeScanner.Scanner;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class ScannerTest
{
	[Test]
	public void TestSettings()
	{
		var settings = new ScannerSettings("Webcam Name")
		{
			ScannerBackgroundThread = false,
			ScannerDelayFrameMin = 2,

			ParserAutoRotate = false,
			ParserTryInverted = false,
			ParserTryHarder = true,

			WebcamRequestedWidth = 256,
			WebcamRequestedHeight = 512,
			WebcamFilterMode = FilterMode.Bilinear
		};

		var scanner = new Scanner(settings);
		Assert.AreSame(scanner.Settings, settings);
	}
}
