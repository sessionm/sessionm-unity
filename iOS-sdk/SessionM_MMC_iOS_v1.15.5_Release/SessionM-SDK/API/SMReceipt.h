//
//  SMReceipt.h
//  SessionM
//
//  Copyright (c) 2016 SessionM. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "SMFeedMessageData.h"
#import "SMOrder.h"

/*!
 @typedef SMReceiptStatus
 @abstract Receipt upload status.
 */
typedef enum SMReceiptStatus {
    /*! Upload is pending review. */
    SMReceiptStatusPending,
    /*! Upload was approved. */
    SMReceiptStatusApproved,
    /*! Upload was rejected. */
    SMReceiptStatusRejected
} SMReceiptStatus;

/*!
 @class SMReceipt
 @abstract Defines the data associated with a receipt uploaded by the user.
 */
@interface SMReceipt : NSObject

/*!
 @property receiptID
 @abstract Unique ID for receipt.
 */
@property(nonatomic, strong, readonly) NSString *receiptID;
/*!
 @property messageID
 @abstract ID of message campaign associated with receipt.
 */
@property(nonatomic, strong, readonly) NSString *messageID;
/*!
 @property imageURL
 @abstract URL of uploaded receipt image.
 */
@property(nonatomic, strong, readonly) NSString *imageURL;
/*!
 @property detailsURL
 @abstract URL of page shown when @link presentDetailsPage @/link is called.
 */
@property(nonatomic, strong, readonly) NSString *detailsURL;
/*!
 @property promotion
 @abstract Promotion data associated with receipt.
 */
@property(nonatomic, strong, readonly) SMFeedMessageData *promotion;
/*!
 @property order
 @abstract Order data associated with receipt.
 */
@property(nonatomic, strong, readonly) SMOrder *order;
/*!
 @property createdTime
 @abstract Denotes when receipt data was created.
 */
@property(nonatomic, strong, readonly) NSString *createdTime;
/*!
 @property updatedTime
 @abstract Denotes when receipt data was last updated.
 */
@property(nonatomic, strong, readonly) NSString *updatedTime;
/*!
 @property reason
 @abstract Describes why upload was rejected.
 */
@property(nonatomic, strong, readonly) NSString *reason;
/*!
 @property status
 @abstract Receipt upload status.
 */
@property(nonatomic, readonly) SMReceiptStatus status;

/*!
 @abstract Presents the content pointed to by @link detailsURL @/link.
 @result BOOL indicating whether details page will be presented.
 */
- (BOOL)presentDetailsPage;
/*!
 @abstract Notifies the SDK that the view containing the associated receipt data was presented to the user. Used for reporting purposes.
 */
- (void)notifyPresented;
/*!
 @abstract Notifies the SDK that the view containing the associated receipt data was seen by the user. Used for reporting purposes.
 */
- (void)notifySeen;
/*!
 @abstract Notifies the SDK that the user tapped on the view containing the associated receipt data. Used for reporting purposes.
 */
- (void)notifyTapped;

@end
