using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class FeedMessageData
{

  private string id;
  private string type;
  private string header;
  private string subheader;
  private string description;
  private string iconURL;
  private string imageURL;
  private string actionURL;
  private string payloads;
  private string startTime;
  private string endTime;

  public FeedMessageData(string id, string type, string header, string subheader, string description,
                         string iconURL, string imageURL, string actionURL,
                         string payloads,
                         string startTime,
                         string endTime) {
    this.id = id;
    this.type = type;
    this.header = header;
    this.subheader = subheader;
    this.description = description;
    this.iconURL = iconURL;
    this.imageURL = imageURL;
    this.actionURL = actionURL;
    this.payloads = payloads;
    this.startTime = startTime;
    this.endTime = endTime;
  }

  public string GetID() { return this.id; }
  public string GetMType() { return this.type; }
  public string GetHeader() { return this.header; }
  public string GetSubHeader() { return this.subheader; }
  public string GetDescription() { return this.description; }
  public string GetIconURL() { return this.iconURL; }
  public string GetImageURL() { return this.imageURL; }
  public string GetActionURL() { return this.actionURL; }
  public string GetPayloads() { return this.payloads; }
  public string GetStartTime() { return this.startTime; }
  public string GetEndTime() { return this.endTime; }
}
