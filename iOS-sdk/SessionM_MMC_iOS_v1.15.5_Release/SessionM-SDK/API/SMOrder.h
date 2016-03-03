//
//  SMOrder.h
//  SessionM
//
//  Copyright (c) 2016 SessionM. All rights reserved.
//

#import <Foundation/Foundation.h>

/*!
 @typedef SMOrderStatus
 @abstract Order status.
 */
typedef enum SMOrderStatus {
    /*! Order is available for redemption. */
    SMOrderStatusAvailable,
    /*! Order was redeemed. */
    SMOrderStatusRedeemed,
    /*! Confirmation that offer was redeemed (for systems that allow batch reconciliation). */
    SMOrderStatusReconciled,
    /*! Order redemption was rejected by Customer Care Team (e.g. due to Terms of Service violation). */
    SMOrderStatusRejected,
    /*! Order is pending review. */
    SMOrderStatusPending,
    /*! Error was received while attempting to redeem order. */
    SMOrderStatusRedemptionError,
    /*! Order is expired. */
    SMOrderStatusExpired
} SMOrderStatus;

/*!
 @class SMOrder
 @abstract Defines the data associated with a reward order made by the user.
 */
@interface SMOrder : NSObject

/*!
 @property orderID
 @abstract Unique ID for order.
 */
@property(nonatomic, strong, readonly) NSString *orderID;
/*!
 @property messageID
 @abstract ID of message campaign associated with order.
 */
@property(nonatomic, strong, readonly) NSString *messageID;
/*!
 @property type
 @abstract Order type.
 */
@property(nonatomic, strong, readonly) NSString *type;
/*!
 @property header
 @abstract Offer title
 */
@property(nonatomic, strong, readonly) NSString *header;
/*!
 @property subheader
 @abstract Offer subtitle.
 */
@property(nonatomic, strong, readonly) NSString *subheader;
/*!
 @property descriptionText
 @abstract Offer text.
 */
@property(nonatomic, strong, readonly) NSString *descriptionText;
/*!
 @property iconURL
 @abstract URL of offer icon.
 */
@property(nonatomic, strong, readonly) NSString *iconURL;
/*!
 @property imageURL
 @abstract URL of offer banner image.
 */
@property(nonatomic, strong, readonly) NSString *imageURL;
/*!
 @property createdTime
 @abstract Denotes when order data was created.
 */
@property(nonatomic, strong, readonly) NSString *createdTime;
/*!
 @property updatedTime
 @abstract Denotes when order data was last updated.
 */
@property(nonatomic, strong, readonly) NSString *updatedTime;
/*!
 @property validUntil
 @abstract Denotes until when offer is valid.
 */
@property(nonatomic, strong, readonly) NSString *validUntil;
/*!
 @property legal
 @abstract Legal terms and conditions for offer.
 */
@property(nonatomic, strong, readonly) NSString *legal;
/*!
 @property data
 @abstract Contains voucher data associated with order.
 */
@property(nonatomic, strong, readonly) NSDictionary *data;
/*!
 @property status
 @abstract Order status.
 */
@property(nonatomic, readonly) SMOrderStatus status;

/*!
 @abstract Updates the order status.
 @discussion This method should be called to notify the SDK of an update to the order status, such as when the user attempts to redeem an offer at a point of sale.
 @param status The new status.
 @param data Order metadata.
 @result BOOL indicating whether status will be updated. Returns <code>NO</code> for invalid status transitions, and <code>YES</code> otherwise.
 */
- (BOOL)updateStatus:(SMOrderStatus)status withData:(NSDictionary *)data;

@end
